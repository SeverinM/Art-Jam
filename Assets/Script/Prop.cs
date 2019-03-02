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
    static int lastCreatedType;

    [SerializeField]
    AnimationCurve curve;

    [SerializeField]
    AnimationCurve sizeCurve;
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
    static Dictionary<int, List<GameObject>> allTypes;

    List<Gaussian> allGauss;
    int spawnCount = 3;

    [SerializeField]
    bool stop;

    [SerializeField]
    float StartLength = 0.001f;
    public float Length => StartLength;

    private void Awake()
    {
        color1 = new Color(Random.value, Random.value, Random.value, 1);
        color2 = new Color(Random.value, Random.value, Random.value, 1);
        if (allTypes == null)
        {
            allTypes = new Dictionary<int, List<GameObject>>();
            for (int i = 0; i < allSpawn.Count; i++)
            {
                allTypes[i] = new List<GameObject>();
            }
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
            allPoints[i].position = (new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * StartLength) + transform.position;
        }        
    }

    // Update is called once per frame
    void Update()
    {
        Duration -= Time.deltaTime;

        if (isPressing) pressedSince += Time.deltaTime;

        for (int i = 0; i < size; i++)
        {
            allPoints[i].random += Random.value * Time.deltaTime;
            Debug.DrawLine(allPoints[i].position, allPoints[i > 0 ? i - 1 : size - 1].position,Color.red);
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
       //Recupere index prefab
        int index;
        Vector3 futurePosition = transform.position + (new Vector3(Mathf.Cos(ind * Mathf.Deg2Rad), 0, Mathf.Sin(ind * Mathf.Deg2Rad)) * Length);
        lastCreatedAbsolute = Instantiate(ReturnRandomList<GameObject>(allSpawn, out index), futurePosition, new Quaternion());

        if (lastCreatedAbsolute.GetComponent<MeshRenderer>())
            FilterColor(lastCreatedAbsolute.GetComponent<MeshRenderer>().material, lastCreatedAbsolute.transform.position);
        if (lastCreatedAbsolute.GetComponent<SkinnedMeshRenderer>())
            FilterColor(lastCreatedAbsolute.GetComponent<SkinnedMeshRenderer>().material, lastCreatedAbsolute.transform.position);

        allTypes[index].Add(lastCreatedAbsolute);
        lastCreatedType = index;
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

            case (ActionsInput.Copy):
                StartCoroutine(Copy());
                break;

            case (ActionsInput.SetColorPerType):
                StartCoroutine(SetColor(type));
                break;

            case (ActionsInput.ModifyShape):
                StartCoroutine(SetShape());
                break;
        }
    }
    #endregion

    IEnumerator Grow()
    {
        Debug.Log("grow");
        if (!Prop.lastCreatedAbsolute)
        {
            int sample = Random.Range(0, size);
            Vector3 virtualPosition = transform.position + (new Vector3(Mathf.Cos(sample * Mathf.Deg2Rad), 0, Mathf.Sin(sample * Mathf.Deg2Rad)) * Length);
            if (Vector3.Distance(origin, transform.position) > Vector3.Distance(origin, virtualPosition))
            {
                sample -= 180;
                sample %= 360;
                Debug.Log("demi tour");
            }
            Spawn(sample);
        }
        else
        {
            int sample = Random.Range(0, size);
            Vector3 virtualPosition = Prop.lastCreatedAbsolute.transform.position + (new Vector3(Mathf.Cos(sample * Mathf.Deg2Rad), 0, Mathf.Sin(sample * Mathf.Deg2Rad)) * Length);
            if (Vector3.Distance(origin, Prop.lastCreatedAbsolute.transform.position) > Vector3.Distance(origin, virtualPosition))
            {
                sample -= 180;
                sample %= 360;
                Debug.Log("demi tour");
            }
            Prop.lastCreatedAbsolute.GetComponent<Prop>().Spawn(sample);
        }

        GameObject gob = Prop.lastCreatedAbsolute;
        float time = 0;
        float timeMax = 2;
        float max = Random.Range(1.5f, 2.5f);
        Vector3 currentSize = gob.transform.localScale;
        Vector3 finalSize = currentSize * max;
        while (time < timeMax)
        {
            gob.transform.localScale = Vector3.Lerp(currentSize, finalSize, sizeCurve.Evaluate(time / timeMax));
            time += Time.deltaTime;
            yield return null;
        }

        yield return null;
    }

    IEnumerator Copy()
    {
        Debug.Log("copy");
        if (!Prop.lastCreatedAbsolute)
        {
            int sample = Random.Range(0, size);
            Vector3 virtualPosition = transform.position + (new Vector3(Mathf.Cos(sample * Mathf.Deg2Rad), 0, Mathf.Sin(sample * Mathf.Deg2Rad)) * Length);

            //La nouvelle position doit etre plus eloigné de la position d'origine
            if (Vector3.Distance(origin, transform.position) > Vector3.Distance(origin, virtualPosition))
            {
                sample -= 180;
                sample %= 360;
                Debug.Log("demi tour");
            }
            Spawn(sample);
        }

        GameObject gob = Prop.lastCreatedAbsolute;
        Vector3 firstPosition = gob.transform.position + new Vector3(Random.Range(0.00001f, gob.GetComponent<Prop>().Length), 0, Random.Range(0.00001f, gob.GetComponent<Prop>().Length));
        Vector3 delta = firstPosition - gob.transform.position;
        int index;
        for (int i = 0; i < 3; i++)
        {
            delta = Quaternion.AngleAxis(Random.Range(-20, 20), Vector3.up) * delta;
            (Instantiate(ReturnRandomList<GameObject>(allSpawn, out index), firstPosition + delta, new Quaternion()) as GameObject).transform.localScale *= Random.Range(0.1f, 1);
        }
        yield return null;
    }

    IEnumerator SetColor(int type)
    {
        Debug.Log("color");
        if (!Prop.lastCreatedAbsolute)
        {
            int sample = Random.Range(0, size);
            Vector3 virtualPosition = transform.position + (new Vector3(Mathf.Cos(sample * Mathf.Deg2Rad), 0, Mathf.Sin(sample * Mathf.Deg2Rad)) * Length);

            //La nouvelle position doit etre plus eloigné de la position d'origine
            if (Vector3.Distance(origin, transform.position) > Vector3.Distance(origin, virtualPosition))
            {
                sample -= 180;
                sample %= 360;
                Debug.Log("demi tour");
            }
            Spawn(sample);
        }
        else
        {
            int sample = Random.Range(0, size);
            Vector3 virtualPosition = Prop.lastCreatedAbsolute.transform.position + (new Vector3(Mathf.Cos(sample * Mathf.Deg2Rad), 0, Mathf.Sin(sample * Mathf.Deg2Rad)) * Length);

            //La nouvelle position doit etre plus eloigné de la position d'origine
            if (Vector3.Distance(origin, Prop.lastCreatedAbsolute.transform.position) > Vector3.Distance(origin, virtualPosition))
            {
                sample -= 180;
                sample %= 360;
                Debug.Log("demi tour");
            }
            Prop.lastCreatedAbsolute.GetComponent<Prop>().Spawn(sample);
        }
        Color col = new Color(Random.value, Random.value, Random.value, 1);
        foreach(GameObject gob in allTypes[type])
        {
            if (gob.GetComponent<MeshRenderer>())
                gob.GetComponent<MeshRenderer>().material.color = col;
            if (gob.GetComponent<SkinnedMeshRenderer>())
                gob.GetComponent<SkinnedMeshRenderer>().material.color = col;
        }
        yield return null;
    }

    IEnumerator SetShape()
    {
        Debug.Log("shape");
        yield return null;
    }
}
