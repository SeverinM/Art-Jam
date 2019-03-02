using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


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

    Color color1;
    Color color2;
    static GameObject lastCreatedAbsolute;
    static int lastCreatedType;
    static Dictionary<int, List<GameObject>> allTypes;
    static float maxRange = 3;
    static Vector3 currentDirection;

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

    int spawnCount = 3;

    [SerializeField]
    bool stop;

    [SerializeField]
    float StartLength = 0.001f;
    public float Length => StartLength;

    static Buffer buff;

    private void Awake()
    {
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

    void Spawn()
    {
        currentDirection = Quaternion.AngleAxis(Random.Range(-20, 20), Vector3.up) * currentDirection;
        Debug.Log(currentDirection);

       //Recupere index prefab
        int index;
        Vector3 futurePosition = transform.position + (currentDirection.normalized * Length);

        if (Vector3.Distance(new Vector3(0,0,0) , futurePosition + new Vector3(Length, 0, Length)) > maxRange)
        {
            Debug.Log(currentDirection);
            float randomVal = (Random.Range(90, 179) * (Random.value > 0.5 ? 1 : -1));
            currentDirection = Quaternion.AngleAxis(randomVal, Vector3.up) * currentDirection;
            futurePosition = transform.position + (currentDirection.normalized * Length);
        }

        lastCreatedAbsolute = Instantiate(ReturnRandomList<GameObject>(allSpawn, out index), futurePosition, new Quaternion());

        if (lastCreatedAbsolute.GetComponent<MeshRenderer>())
            FilterColor(lastCreatedAbsolute.GetComponent<MeshRenderer>().material, lastCreatedAbsolute.transform.position);
        if (lastCreatedAbsolute.GetComponent<SkinnedMeshRenderer>())
        {
            FilterColor(lastCreatedAbsolute.GetComponent<SkinnedMeshRenderer>().material, lastCreatedAbsolute.transform.position);
        }
            

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
            futurePosition = lastCreatedAbsolute.transform.position + (delta.normalized * Length);
            lastCreatedAbsolute = Instantiate(ReturnRandomList<GameObject>(allSpawn, out index), futurePosition, new Quaternion());
        }
    }

    #region modification

    public void FilterColor(Material mat, Vector3 position)
    {
        Color sampledColor = Random.ColorHSV(0, 1, 35 / 255.0f, 35 / 255.0f);
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
        if (!Prop.lastCreatedAbsolute)
        {
            int sample = Random.Range(0, size);
            Vector3 virtualPosition = transform.position + (new Vector3(Mathf.Cos(sample * Mathf.Deg2Rad), 0, Mathf.Sin(sample * Mathf.Deg2Rad)) * Length);
            if (Vector3.Distance(origin, transform.position) > Vector3.Distance(origin, virtualPosition))
            {
                currentDirection = Quaternion.AngleAxis(180, Vector3.up) * currentDirection;
            }
            Spawn();
        }
        else
        {
            int sample = Random.Range(0, size);
            Vector3 virtualPosition = Prop.lastCreatedAbsolute.transform.position + (new Vector3(Mathf.Cos(sample * Mathf.Deg2Rad), 0, Mathf.Sin(sample * Mathf.Deg2Rad)) * Length);
            if (Vector3.Distance(origin, Prop.lastCreatedAbsolute.transform.position) > Vector3.Distance(origin, virtualPosition))
            {
                currentDirection = Quaternion.AngleAxis((Random.Range(90, 179) * Random.value > 0.5 ? 1 : -1), Vector3.up) * currentDirection;
            }
            Prop.lastCreatedAbsolute.GetComponent<Prop>().Spawn();
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
        if (!Prop.lastCreatedAbsolute)
        {
            int sample = Random.Range(0, size);
            Vector3 virtualPosition = transform.position + (new Vector3(Mathf.Cos(sample * Mathf.Deg2Rad), 0, Mathf.Sin(sample * Mathf.Deg2Rad)) * Length);

            //La nouvelle position doit etre plus eloigné de la position d'origine
            if (Vector3.Distance(origin, transform.position) > Vector3.Distance(origin, virtualPosition))
            {
                currentDirection = Quaternion.AngleAxis(180, Vector3.up) * currentDirection;
            }
            Spawn();
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
        if (!Prop.lastCreatedAbsolute)
        {
            int sample = Random.Range(0, size);
            Vector3 virtualPosition = transform.position + (new Vector3(Mathf.Cos(sample * Mathf.Deg2Rad), 0, Mathf.Sin(sample * Mathf.Deg2Rad)) * Length);

            //La nouvelle position doit etre plus eloigné de la position d'origine
            if (Vector3.Distance(origin, transform.position) > Vector3.Distance(origin, virtualPosition))
            {
                currentDirection = Quaternion.AngleAxis(180, Vector3.up) * currentDirection;
            }
            Spawn();
        }
        else
        {
            int sample = Random.Range(0, size);
            Vector3 virtualPosition = Prop.lastCreatedAbsolute.transform.position + (new Vector3(Mathf.Cos(sample * Mathf.Deg2Rad), 0, Mathf.Sin(sample * Mathf.Deg2Rad)) * Length);

            //La nouvelle position doit etre plus eloigné de la position d'origine
            if (Vector3.Distance(origin, Prop.lastCreatedAbsolute.transform.position) > Vector3.Distance(origin, virtualPosition))
            {
                currentDirection = Quaternion.AngleAxis(180, Vector3.up) * currentDirection;
            }
            Prop.lastCreatedAbsolute.GetComponent<Prop>().Spawn();
        }
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

    IEnumerator SetShape()
    {

        if (!Prop.lastCreatedAbsolute)
        {
            int sample = Random.Range(0, size);
            Vector3 virtualPosition = transform.position + (new Vector3(Mathf.Cos(sample * Mathf.Deg2Rad), 0, Mathf.Sin(sample * Mathf.Deg2Rad)) * Length);

            //La nouvelle position doit etre plus eloigné de la position d'origine
            if (Vector3.Distance(origin, transform.position) > Vector3.Distance(origin, virtualPosition))
            {
                //Rotation 180
                currentDirection = Quaternion.AngleAxis(180, Vector3.up) * currentDirection;
            }
            Spawn();
        }
        GameObject gob = Prop.lastCreatedAbsolute;
        float time = 0;
        float timeMax = 2;
        float max = Random.Range(0.5f, 1.5f);
        Vector3 currentSize = gob.transform.localScale;
        Vector3 finalSize = currentSize * max;
        while (time < timeMax)
        {
            gob.transform.localScale = Vector3.Lerp(currentSize, finalSize, sizeCurve.Evaluate(time / timeMax));
            time += Time.deltaTime;

            if (GetComponent<SkinnedMeshRenderer>())
            {
                GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, (time / timeMax) * 100);
            }
            yield return null;
        };

        
        yield return null;
    }
}
