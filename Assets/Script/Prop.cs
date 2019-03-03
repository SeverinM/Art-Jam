using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Collections.ObjectModel;


public enum ActionsInput
{
    SetColorPerType,
    BiggerCell,
    ModifyShape,
    Copy,
    NoInteract
}

public class Prop : MonoBehaviour
{
    float Duration = 10;

    Color color1;
    Color color2;
    public static GameObject lastCreatedAbsolute;
    static int lastCreatedType;
    static Dictionary<int, List<GameObject>> allTypes;
    public static float maxRange = 7;
    static Vector3 currentDirection;
    static bool someoneGrowing = false;

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
    List<GameObject> AllSpawn => allSpawn;

    [SerializeField]
    float freqPerlin = 0.01f;

    //Tous les GO de chaque type

    int spawnCount = 3;

    [SerializeField]
    bool stop;

    [SerializeField]
    float StartLength = 0.001f;
    public float Length => StartLength;

    static Buffer buff;

    private void Awake()
    {
        if (lastCreatedAbsolute == null)
        {
            allSpawn = allSpawn.OrderBy(x => Random.value).ToList();
            allSpawn.RemoveRange(5, 3);
        }

        if (buff == null)
        {
            buff = new Buffer();
        }
        
        if (currentDirection == Vector3.zero)
        {
            float angle = Random.Range(0, 359);
            currentDirection = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
        }


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

    T ReturnRandomList<T>(List<T> list, out int index)
    {
        index = Random.Range(0, list.Count);
        return list[index];
    }

    void CopySpawn(Prop destination)
    {
        destination.allSpawn.Clear();
        foreach(GameObject gob in allSpawn)
        {
            destination.allSpawn.Add(gob);
        }
    }

    void PlaySound(int index)
    {
        string value = "";
        switch (index)
        {
            case 0:
                value = "Play_Mic_01";
                break;
            case 1:
                value = "Play_Mic_02";
                break;
            case 2:
                value = "Play_Mic_05";
                break;
            case 3:
                value = "Play_Mic_04";
                break;
            case 4:
                value = "Play_Mic_03";
                break;
            default:
                value = "Play_Mic_01";
                break;
        }
        AkSoundEngine.PostEvent(value, Camera.main.gameObject);
    }

    void Spawn(int ind, bool playSound = true)
    {
        currentDirection.y = 0;
        currentDirection = Quaternion.AngleAxis(Random.Range(-20, 20), Vector3.up) * currentDirection;
        currentDirection = currentDirection.normalized * StartLength;
        if (playSound) PlaySound(ind);

        //Recupere index prefab
        int index;
        Vector3 futurePosition = transform.position + (currentDirection.normalized * Length);

        if (Vector3.Distance(new Vector3(0,0,0) , futurePosition) > maxRange)
        {
            currentDirection = Quaternion.AngleAxis(180, Vector3.up) * currentDirection;
            currentDirection = Quaternion.AngleAxis(Random.Range(-10,10), Vector3.up) * currentDirection;
            currentDirection = currentDirection.normalized * StartLength;
            futurePosition = transform.position + (currentDirection.normalized * Length);
            Debug.Log("fin");
        }

        lastCreatedAbsolute = gameObject;
        lastCreatedAbsolute = Instantiate(ReturnRandomList<GameObject>(allSpawn, out index), futurePosition, new Quaternion());
        lastCreatedAbsolute.transform.localScale *= Random.Range(0.5f, 1.5f);
        lastCreatedAbsolute.transform.Rotate(Vector3.up, Random.Range(0, 360));
        CopySpawn(lastCreatedAbsolute.GetComponent<Prop>());

        if (lastCreatedAbsolute.GetComponent<MeshRenderer>())
            FilterColor(lastCreatedAbsolute.GetComponent<MeshRenderer>().material, lastCreatedAbsolute.transform.position);
        if (lastCreatedAbsolute.GetComponent<SkinnedMeshRenderer>())
        {
            FilterColor(lastCreatedAbsolute.GetComponent<SkinnedMeshRenderer>().material, lastCreatedAbsolute.transform.position);
        }

        if (!allTypes.ContainsKey(index))
            allTypes[index] = new List<GameObject>();
        allTypes[index].Add(lastCreatedAbsolute);
        lastCreatedType = index;

        //Bufferisation
        buff.TryStore(lastCreatedAbsolute);

        //Ramification
        if (buff.TryReplaceLast() && buff.Stored != null && buff.Stored != this.gameObject)
        {
            lastCreatedAbsolute = buff.Stored;
            Vector3 delta = transform.position;
            currentDirection = Quaternion.AngleAxis(90 * (Random.value > 0.5 ? 1 : -1), Vector3.up) * (lastCreatedAbsolute.transform.position - origin);
            currentDirection = currentDirection.normalized * StartLength;
            currentDirection.y = 0;
            futurePosition = lastCreatedAbsolute.transform.position + (delta.normalized * Length);
            lastCreatedAbsolute = Instantiate(ReturnRandomList<GameObject>(allSpawn, out index), futurePosition, new Quaternion());
        }
    }

    #region modification

    public void FilterColor(Material mat, Vector3 position)
    {
        Color sampledColor = Random.ColorHSV(0, 1, 45 / 255.0f, 45 / 255.0f);
        mat.color = sampledColor;
    }

    public void InterpretInput(ActionsInput act, int type)
    {
        Random.seed += type;
        switch (act)
        {
            case (ActionsInput.BiggerCell):
                StartCoroutine(Grow(type));
                break;

            case (ActionsInput.Copy):
                StartCoroutine(Copy(type));
                break;

            case (ActionsInput.SetColorPerType):
                StartCoroutine(SetColor(type));
                break;

            case (ActionsInput.ModifyShape):
                StartCoroutine(SetShape(type));
                break;

            case (ActionsInput.NoInteract):
                StartCoroutine(SlightShape());
                break;
        }
    }

    IEnumerator SlightShape()
    {
        float sample = Mathf.PerlinNoise(Time.timeSinceLevelLoad * 0.002f, 0) - 0.5f;
        sample *= 2;
        foreach (SkinnedMeshRenderer skinned in GameObject.FindObjectsOfType<SkinnedMeshRenderer>())
        {
            skinned.SetBlendShapeWeight(0, skinned.GetBlendShapeWeight(0) + sample);
        }
        yield return null;
    }

    IEnumerator Grow(int ind)
    {   
        if (!Prop.lastCreatedAbsolute)
        {
            Spawn(ind);
        }
        else
        {
            Prop.lastCreatedAbsolute.GetComponent<Prop>().Spawn(ind);
        }

        GameObject gob = Prop.lastCreatedAbsolute;
        float time = 0;
        float timeMax = 7;
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

    IEnumerator Copy(int ind)
    {
        AkSoundEngine.PostEvent("Play_Copy", Camera.main.gameObject);
        GameObject gob = Prop.lastCreatedAbsolute;
        Vector3 firstPosition = gob.transform.position + new Vector3(Random.Range(0.00001f, gob.GetComponent<Prop>().Length), 0, Random.Range(0.00001f, gob.GetComponent<Prop>().Length));
        Vector3 delta = firstPosition - gob.transform.position;
        for (int i = 0; i < 3; i++)
        {
            delta = Quaternion.AngleAxis(Random.Range(-20, 20), Vector3.up) * delta;
            (Instantiate(Prop.lastCreatedAbsolute, firstPosition + delta, new Quaternion()) as GameObject).transform.localScale *= Random.Range(0.1f, 1);

        }
        yield return null;
    }

    IEnumerator SetColor(int type)
    {
        AkSoundEngine.PostEvent("Play_Color", Camera.main.gameObject);
        Color col = Random.ColorHSV(0, 1, 35 / 255.0f, 35 / 255.0f);

        foreach (GameObject gob in allTypes[type])
        {
            if (gob.GetComponent<MeshRenderer>())
                gob.GetComponent<MeshRenderer>().material.color = col;
            if (gob.GetComponent<SkinnedMeshRenderer>())
                gob.GetComponent<SkinnedMeshRenderer>().material.color = col;
        }
        yield return null;
    }

    IEnumerator SetShape(int ind)
    {
        AkSoundEngine.PostEvent("Play_Morphing", Camera.main.gameObject);
        float time = 0;
        float timeMax = 2;
        while (time < timeMax)
        {

            foreach (SkinnedMeshRenderer skinned in GameObject.FindObjectsOfType<SkinnedMeshRenderer>())
            {
                skinned.SetBlendShapeWeight(0, skinned.GetBlendShapeWeight(0) + (Random.value * curve.Evaluate(time / timeMax) * 0.5f));
            }
            time += Time.deltaTime;
            yield return null;
        };

        
        yield return null;
    }

    #endregion
}
