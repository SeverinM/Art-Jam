using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBackward : MonoBehaviour
{
    [SerializeField]
    AnimationCurve cameraAnim;
    float speed;
    float x;
    bool acting;
    float xTolerance;
    float yTolerance;

    //Position avant transition
    Vector3 origin;
    Vector3 end;
    GameObject last;

    private void Awake()
    {
        x = 0;
        speed = 0.5f;
        acting = false;
        xTolerance = 0.8f;
        yTolerance = 0.8f;
    }

    // Update is called once per frame
    void Update()
    {
        if (acting)
        {
            x += Time.deltaTime * speed;
            transform.position = Vector3.Lerp(origin, end, cameraAnim.Evaluate(x));
            if (x > 1)
            {
                acting = false;
            }
        }

        if (Prop.lastCreatedAbsolute == null || Prop.lastCreatedAbsolute == last) return;

        last = Prop.lastCreatedAbsolute;
        Vector3 camPosition = GetComponent<Camera>().WorldToScreenPoint(Prop.lastCreatedAbsolute.transform.position);
        if (isInBorder(camPosition) && !acting)
        {
            acting = true;
            origin = transform.position;
            end = transform.position + (Vector3.up * 2);
            x = 0;
        }
    }

    bool isInBorder(Vector3 screenPos)
    {   
        //Bordure de l'ecran
        if (screenPos.x / GetComponent<Camera>().pixelWidth < (1 - xTolerance) || screenPos.x / GetComponent<Camera>().pixelWidth > xTolerance ||
            screenPos.y / GetComponent<Camera>().pixelHeight < (1 - yTolerance) || screenPos.y / GetComponent<Camera>().pixelHeight > yTolerance)
        {
            return true;
        }

        return false;
    }
}
