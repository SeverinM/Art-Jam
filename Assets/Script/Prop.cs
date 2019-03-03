﻿using System.Collections;
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
    static float maxRange = 5;
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

    void CopySpawn(Prop origin)
    {
        allSpawn.Clear();
        foreach(GameObject gob in origin.allSpawn)
        {
            allSpawn.Add(gob);
        }
    }

    void Spawn()
    {
        currentDirection = Quaternion.AngleAxis(Random.Range(-20, 20), Vector3.up) * currentDirection;

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
        lastCreatedAbsolute.transform.localScale *= Random.Range(0.5f, 1.5f);
        lastCreatedAbsolute.transform.Rotate(Vector3.up, Random.Range(0, 360));
        lastCreatedAbsolute.GetComponent<Prop>().CopySpawn(this);

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

    IEnumerator Grow()
    {
        if (!Prop.lastCreatedAbsolute)
        {
            Spawn();
        }
        else
        {
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
            Spawn();
        }

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
        if (!Prop.lastCreatedAbsolute)
        {
            Spawn();
        }
        else
        {
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

            foreach (SkinnedMeshRenderer skinned in GameObject.FindObjectsOfType<SkinnedMeshRenderer>())
            {
                skinned.SetBlendShapeWeight(0, skinned.GetBlendShapeWeight(0) + (Random.value * curve.Evaluate(time / timeMax) * 2));
            }
            gob.transform.localScale = Vector3.Lerp(currentSize, finalSize, sizeCurve.Evaluate(time / timeMax));
            time += Time.deltaTime;
            yield return null;
        };

        
        yield return null;
    }

    #endregion
}
