using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

struct PointPropagation
{
    //Position du point sur la surface
    public Vector3 position;

    //Pour faire avancer le point plus vite que les autres
    public float modifier;

    //Utilisé pour le spawn
    public float random;

    public float randomSecondary;
    public GameObject lastCreated;
}

struct Gaussian
{
    public float variance;
    public float middle;
}

public enum ActionsInput
{
    SetColorPerType,
    BiggerCell,
    ModifyShape,
    Copy,
}

public class Prop : MonoBehaviour
{
    float Duration = 10;
    PointPropagation[] allPoints;

    Color color1;
    Color color2;
    static GameObject lastCreatedAbsolute;
    int lastCreatedType;

    [SerializeField]
    AnimationCurve curve;
    float pressedSince;
    bool isPressing;

    [SerializeField]
    int size = 360;

    Vector3 origin;

    [SerializeField]
    List<GameObject> allSpawn;

    [SerializeField]
    float freqPerlin = 0.01f;

    //Tous les GO de chaque type
    static Dictionary<int, List<GameObject>> allTypes = new Dictionary<int, List<GameObject>>();

    List<Gaussian> allGauss;
    int spawnCount = 3;

    [SerializeField]
    bool stop;

    [SerializeField]
    float StartLength = 0.001f;

    private void Awake()
    {
        color1 = new Color(Random.value, Random.value, Random.value, 1);
        color2 = new Color(Random.value, Random.value, Random.value, 1);
        for (int i = 0; i < allSpawn.Count; i++)
        {
            allTypes[i] = new List<GameObject>();
        }

        lastCreatedType = -1;
        origin = transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        Gaussian gauss = new Gaussian();
        allGauss = new List<Gaussian>();

        for (int i = 0; i < 5; i++)
        {
            gauss.variance = Random.Range(1, 10);
            gauss.middle = Random.Range(0, 360);
            //allGauss.Add(gauss);
        }

        allPoints = new PointPropagation[size];

        for (int i = 0; i < size; i++)
        {
            float angle = i * ((Mathf.PI * 2) / size);
            allPoints[i].position = (new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * StartLength) + origin;
            UpdateGauss(i);
            allPoints[i].random = Random.Range(-10, 5);
            allPoints[i].randomSecondary = Random.Range(5, 30);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Duration -= Time.deltaTime;

        if (isPressing) pressedSince += Time.deltaTime;

        for (int i = 0; i < size; i++)
        {
            allPoints[i].position += (allPoints[i].position - origin).normalized * (allPoints[i].modifier) * (stop ? 0 : 1); 
            allPoints[i].random += Random.value * Time.deltaTime;
            //Debug.DrawLine(allPoints[i].position, allPoints[i > 0 ? i - 1 : size - 1].position,Color.red);
        }

        if (spawnCount == 0)
            Destroy(this);
    }

    T ReturnRandomList<T>(List<T> list, out int index)
    {
        index = Random.Range(0, list.Count);
        return list[index];
    }

    float CalculateGaussian(float x, Gaussian gauss)
    {
        float v1 = Mathf.Pow(((x - gauss.middle) / gauss.variance), 2) / -2;
        float v2 = Mathf.Exp(v1);
        float v3 = (1 / (gauss.variance * Mathf.Sqrt(2 * Mathf.PI))) * v2;
        return v3;
    }

    Vector3 Sample(float angle)
    {
        float frac = angle - Mathf.Floor(angle);
        int ang = (int)angle;
        return Vector3.Lerp(allPoints[ang - 1].position , allPoints[ang].position, frac);

    }

    void Spawn(int ind)
    {
        allPoints[ind].randomSecondary = Random.Range(0, 10);
        Vector3 previous = allPoints[ind > 0 ? ind - 1 : size - 1].position;
        allPoints[ind].random = 0;

        //Recupere index prefab
        int index;
        allPoints[ind].lastCreated = Instantiate(ReturnRandomList<GameObject>(allSpawn, out index), Vector3.Lerp(previous, allPoints[ind].position, Random.value), new Quaternion());
        allPoints[ind].lastCreated.transform.localScale *= (Mathf.PerlinNoise(allPoints[ind].position.x / freqPerlin, allPoints[ind].position.z / freqPerlin) + 0.5f);
        spawnCount--;

        if (allPoints[ind].lastCreated.GetComponent<MeshRenderer>())
            FilterColor(allPoints[ind].lastCreated.GetComponent<MeshRenderer>().material, allPoints[ind].position);
        if (allPoints[ind].lastCreated.GetComponent<SkinnedMeshRenderer>())
            FilterColor(allPoints[ind].lastCreated.GetComponent<SkinnedMeshRenderer>().material, allPoints[ind].position);

        lastCreatedAbsolute = allPoints[ind].lastCreated;
        allTypes[index].Add(lastCreatedAbsolute);
        lastCreatedType = index;
        if (lastCreatedAbsolute.GetComponent<SkinnedMeshRenderer>())
        {
            lastCreatedAbsolute.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, 1);
            lastCreatedAbsolute.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(1, 1);
            lastCreatedAbsolute.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(2, 1);
        }
    }

    #region modification
    public void UpdateGauss(int index)
    {
        float sum = 0;
        allGauss.ForEach(x => sum += CalculateGaussian(Mathf.Abs(index - size / 2), x));
        allPoints[index].modifier = 0.001f + (sum * 0.01f);
    }

    public void FilterColor(Material mat, Vector3 position)
    {
        Color sampledColor = Color.Lerp(color1, color2, (Mathf.PerlinNoise(position.x / freqPerlin, position.z / freqPerlin)));
        mat.color = sampledColor;
    }

    public void InterpretInput(ActionsInput act, int type)
    {
        switch (act)
        {
            case (ActionsInput.BiggerCell):
                StartCoroutine(Grow());
                break;
        }
    }
    #endregion

    IEnumerator Grow()
    {
        GameObject gob = Prop.lastCreatedAbsolute;
        float time = 0;
        float previousVal = 0;
        while (time < 2)
        {
            float value = Mathf.Log(time * 10);
            time += Time.deltaTime;
            gob.transform.localScale += new Vector3(value - previousVal, value - previousVal, value - previousVal);
            previousVal = value;
            yield return null;
        }

        yield return null;
    }

    IEnumerable Copy()
    {
        yield return null;
    }
}
