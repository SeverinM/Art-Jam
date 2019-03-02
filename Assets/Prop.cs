using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct PointPropagation
{
    //Position du point sur la surface
    public Vector3 position;

    //Pour faire avancer le point plus vite que les autres
    public float modifier;

    //Utilisé pour le spawn
    public float random;
}

public class Prop : MonoBehaviour
{
    PointPropagation[] allPoints;

    [SerializeField]
    int size;
    Vector3 origin;

    [SerializeField]
    List<GameObject> allSpawn;

    // Start is called before the first frame update
    void Start()
    {
        allPoints = new PointPropagation[size];
        origin = transform.position;

        for (int i = 0; i < size; i++)
        {
            float angle = i * ((Mathf.PI * 2) / size);
            allPoints[i].position = new Vector3(Mathf.Cos(angle),0, Mathf.Sin(angle)) + origin;
            allPoints[i].modifier = Random.value + 0.01f;
            allPoints[i].random = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < size; i++)
        {
            allPoints[i].position += (allPoints[i].position - origin).normalized * allPoints[i].modifier * 0.01f;
            allPoints[i].random += Random.value;
            Debug.DrawLine(allPoints[i].position, allPoints[i > 0 ? i - 1 : size - 1].position,Color.red);

            //Spawn un truc
            if (allPoints[i].random > 50)
            {
                Vector3 previous = allPoints[i > 0 ? i - 1 : size - 1].position;
                allPoints[i].random = Random.Range(0, 40);             
                Instantiate(ReturnRandomList<GameObject>(allSpawn), Vector3.Lerp(previous, allPoints[i].position, Random.value), new Quaternion());
            }
        }
    }

    T ReturnRandomList<T>(List<T> list)
    {
        return list[Random.Range(0, list.Count)];
    }

    void GetPreviousPosition(out Vector3 previous,int index)
    {
        index--;
        if (index < 0 )
        {
            index = size - 1;
        }
        previous = allPoints[index].position;
    }
}
