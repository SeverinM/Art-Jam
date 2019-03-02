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
}

struct Gaussian
{
    public float variance;
    public float middle;
}

public class Prop : MonoBehaviour
{
    PointPropagation[] allPoints;

    [SerializeField]
    int size;
    Vector3 origin;

    [SerializeField]
    List<GameObject> allSpawn;

    List<Gaussian> allGauss;

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
        origin = transform.position;

        for (int i = 0; i < size; i++)
        {
            float angle = i * ((Mathf.PI * 2) / size);
            allPoints[i].position = new Vector3(Mathf.Cos(angle),0, Mathf.Sin(angle)) + origin;
            UpdateGauss(i);
            allPoints[i].random = Random.Range(-10, 5);
            allPoints[i].randomSecondary = Random.Range(0, 10);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            AddVerticalExtension(5);
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            AddHorizontalExtension(5);
        }

        for (int i = 0; i < size; i++)
        {
            allPoints[i].position += (allPoints[i].position - origin).normalized * allPoints[i].modifier;
            allPoints[i].random += Random.value * Time.deltaTime;
            Debug.DrawLine(allPoints[i].position, allPoints[i > 0 ? i - 1 : size - 1].position,Color.red);

            //Spawn un truc
            if (allPoints[i].random > 2)
            {
                allPoints[i].randomSecondary--;
                allPoints[i].random = 0;
                if (allPoints[i].randomSecondary == 0)
                {
                    allPoints[i].randomSecondary = Random.Range(0, 10);
                    Vector3 previous = allPoints[i > 0 ? i - 1 : size - 1].position;
                    allPoints[i].random = 0;
                    Instantiate(ReturnRandomList<GameObject>(allSpawn), Vector3.Lerp(previous, allPoints[i].position, Random.value), new Quaternion());
                }
            }
        }
    }

    T ReturnRandomList<T>(List<T> list)
    {
        return list[Random.Range(0, list.Count)];
    }

    float CalculateGaussian(float x, Gaussian gauss)
    {
        float v1 = Mathf.Pow(((x - gauss.middle) / gauss.variance), 2) / -2;
        float v2 = Mathf.Exp(v1);
        float v3 = (1 / (gauss.variance * Mathf.Sqrt(2 * Mathf.PI))) * v2;
        return v3;
    }

    void UpdateGauss(int index)
    {
        float sum = 0;
        allGauss.ForEach(x => sum += CalculateGaussian(Mathf.Abs(index - size / 2), x));
        allPoints[index].modifier = 0.001f + (sum * 0.01f);
    }

    void AddVerticalExtension(float intensity)
    {
        Gaussian gauss;
        gauss.middle = 90;
        gauss.variance = intensity;
        allGauss.Add(gauss);

        gauss.middle = 270;
        gauss.variance = intensity;
        allGauss.Add(gauss);

        for (int i = 0; i < size; i++)
        {
            UpdateGauss(i);
        }
    }

    void AddHorizontalExtension(float intensity)
    {
        Gaussian gauss;
        gauss.middle = 0;
        gauss.variance = intensity;
        allGauss.Add(gauss);

        gauss.middle = 180;
        gauss.variance = intensity;
        allGauss.Add(gauss);

        for (int i = 0; i < size; i++)
        {
            UpdateGauss(i);
        }
    }
}
