using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public Prop prop;

    //public GameObject cube;
    //public GameObject capsule;
    //public GameObject lastObj;

    //public float scaleIntensity = 1.5f;

    //private float maxScale = 2.0f;
    //private float minScale = 0.1f;

    private Dictionary<string, int> keyCellTypeDic;

    private Dictionary<string, bool> KeyDownDic;
    private bool keyDown = false;

    //private string lastKey = "";
    //private string nextKey = "";
    
    private float counterBetweenKeys;
    
    void Start()
    {
        prop = GetComponent<Prop>();

        counterBetweenKeys = 0.0f;

        keyCellTypeDic = new Dictionary<string, int>();
        keyCellTypeDic["a"] = 0;
        keyCellTypeDic["z"] = 1;
        keyCellTypeDic["e"] = 2;
        keyCellTypeDic["r"] = 0;
        keyCellTypeDic["t"] = 1;
        keyCellTypeDic["y"] = 2;
        
        KeyDownDic = new Dictionary<string, bool>();
        KeyDownDic["k"] = false;
        KeyDownDic["j"] = false;
        
    }

    private bool isKeyDown(string key)
    {
        return KeyDownDic[key];
    }

    private void setKeyDown(string key, bool status)
    {
        KeyDownDic[key] = status;
    }

    private ActionsInput GetActionInputFromTime()
    {
        ActionsInput actionsInput = ActionsInput.BiggerCell;
        if (counterBetweenKeys < 0.5f)
        {
            actionsInput = ActionsInput.SetColorPerType;
        }
        else if(counterBetweenKeys < 1.0f)
        {
            actionsInput = ActionsInput.BiggerCell;
        }
        else if (counterBetweenKeys < 1.5f)
        {
            actionsInput = ActionsInput.ModifyShape;
        }
        else if (counterBetweenKeys < 2.0f)
        {
            actionsInput = ActionsInput.Copy;
        }
        return actionsInput;
    }

    void Update()
    {
        //Debug.Log("Update");

        if(!keyDown)
        {
            counterBetweenKeys += Time.deltaTime;
        }

        //////  A  //////
        if (Input.GetKeyDown("a"))
        {
            keyDown = true;
        }
        if (Input.GetKeyUp("a"))
        {
            // GET ACTION WITH TIME
            

            Debug.Log("Key : a");
            // POP AND DO ACTION
            prop.InterpretInput(ActionsInput.Copy, keyCellTypeDic["a"]);

            keyDown = false;
            counterBetweenKeys = 0.0f;
        }

        //////  K  //////
        //if (Input.GetKeyDown("k"))
        //{
        //    setKeyDown("k", true);
        //    keyDown = true;
        //    test_PopNScale();
        //}
        //if (Input.GetKey("k"))
        //{
        //    test_scale(lastObj);
        //}
        //if (Input.GetKeyUp("k"))
        //{
        //    setKeyDown("k", false);
        //    keyDown = false;
        //    counterBetweenKeys = 0.0f;
        //}
    }

    //void test_PopNScale()
    //{
    //    GameObject cubeObj = Instantiate(lastObj);
    //    cubeObj.transform.position = new Vector3(lastObj.transform.position.x + 0.5f, lastObj.transform.position.y + 0.5f, lastObj.transform.position.z + 0.5f);

    //    float scale = counterBetweenKeys * scaleIntensity;
    //    cubeObj.transform.localScale *= Mathf.Clamp(scale, minScale, maxScale);

    //    lastObj = cubeObj;
    //}

    //void test_PopCube()
    //{
    //    GameObject cubeObj = Instantiate(cube);

    //    float x = Random.Range(-10.0f, 10.0f);
    //    float y = Random.Range(-10.0f, 10.0f);
    //    float z = Random.Range(-10.0f, 10.0f);

    //    float r = Random.Range(0.0f, 1.0f);
    //    float g = Random.Range(0.0f, 1.0f);
    //    float b = Random.Range(0.0f, 1.0f);

    //    cubeObj.transform.position = new Vector3(x, y, z);
    //    cubeObj.GetComponent<Renderer>().material.color = new Color(r, g, b);

    //    lastObj = cubeObj;
    //}

    //void test_PopCapsule()
    //{
    //    GameObject capsuleObj = Instantiate(capsule);

    //    float x = Random.Range(-10.0f, 10.0f);
    //    float y = Random.Range(-10.0f, 10.0f);
    //    float z = Random.Range(-10.0f, 10.0f);

    //    float r = Random.Range(0.0f, 1.0f);
    //    float g = Random.Range(0.0f, 1.0f);
    //    float b = Random.Range(0.0f, 1.0f);

    //    capsuleObj.transform.position = new Vector3(x, y, z);
    //    capsuleObj.GetComponent<Renderer>().material.color = new Color(r, g, b);
    //}

    //void test_scale(GameObject obj)
    //{
    //    obj.transform.localScale = new Vector3(obj.transform.localScale.x + Time.deltaTime, obj.transform.localScale.y + Time.deltaTime, obj.transform.localScale.z + Time.deltaTime);
    //}

    //void test_color(GameObject obj)
    //{
    //    float r = Random.Range(0.0f, 1.0f);
    //    float g = Random.Range(0.0f, 1.0f);
    //    float b = Random.Range(0.0f, 1.0f);
    //    obj.GetComponent<Renderer>().material.color = new Color(r, g, b);
    //}
}
