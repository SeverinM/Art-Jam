using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public GameObject cube;
    public GameObject capsule;

    void test_PopCube()
    {
        GameObject cubeObj = Instantiate(cube);

        float x = Random.Range(-10.0f, 10.0f);
        float y = Random.Range(-10.0f, 10.0f);
        float z = Random.Range(-10.0f, 10.0f);

        float r = Random.Range(0.0f, 1.0f);
        float g = Random.Range(0.0f, 1.0f);
        float b = Random.Range(0.0f, 1.0f);

        cubeObj.transform.position = new Vector3(x, y, z);
        cubeObj.GetComponent<Renderer>().material.color = new Color(r, g, b);
    }

    void test_PopCapsule()
    {
        GameObject capsuleObj = Instantiate(capsule);

        float x = Random.Range(-10.0f, 10.0f);
        float y = Random.Range(-10.0f, 10.0f);
        float z = Random.Range(-10.0f, 10.0f);

        float r = Random.Range(0.0f, 1.0f);
        float g = Random.Range(0.0f, 1.0f);
        float b = Random.Range(0.0f, 1.0f);

        capsuleObj.transform.position = new Vector3(x, y, z);
        capsuleObj.GetComponent<Renderer>().material.color = new Color(r, g, b);
    }

    void Update()
    {
        if (Input.GetKeyUp("k"))
        {
            test_PopCube();
        }

        if (Input.GetKeyUp("j"))
        {
            test_PopCapsule();
        }
    }
}
