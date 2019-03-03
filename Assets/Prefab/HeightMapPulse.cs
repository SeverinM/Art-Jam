using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightMapPulse : MonoBehaviour {
    public float strengh;
    public float speed;
    private Renderer r;

    void Start() {
        r = GetComponent<Renderer>();
        strengh =  Random.Range(2f, 3f);
        speed =  Random.Range(0.5f, 1.5f);
    }

    void Update() {
        float sinus = Mathf.Sin(Time.timeSinceLevelLoad * speed);
        sinus = (sinus + 1f) / strengh;        
        r.material.SetFloat("_Parallax", sinus);
    }
}