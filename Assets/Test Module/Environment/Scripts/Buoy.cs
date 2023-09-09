using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buoy : MonoBehaviour
{
    public float spinSpeed = 1;
    private float spinCounter = 0;


    void Update()
    {
        transform.rotation = Quaternion.Euler(0, spinCounter, 0);
        spinCounter += spinSpeed;
    }
}
