using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buffer
{
    float currentValue;
    GameObject stored;
    public GameObject Stored => stored;
    float testValue = 5;
    int count;
    int maxCount;

    public Buffer()
    {
        currentValue = 0;
        maxCount = (int)Random.Range(10, 12);
    }

    public void TryStore(GameObject gob)
    {
        count++;
        if (count >= maxCount)
        {
            count = 0;
            stored = gob;
            maxCount = (int)Random.Range(10, 12);
        }
    }

    public bool TryReplaceLast()
    {
        currentValue += Random.value;
        if (currentValue > testValue)
        {
            currentValue = 0;
            count = 0;
            return true;
        }
        return false;
    }
    
}
