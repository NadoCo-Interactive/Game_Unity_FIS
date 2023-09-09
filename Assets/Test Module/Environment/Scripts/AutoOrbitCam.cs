using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoOrbitCam : MonoBehaviour
{
    public Vector3 center = Vector3.zero;
    public Vector3 offset = new Vector3(-3, 5, -5);
    public float orbitSpeed = 1;
    private float counter = 0;
    void Start()
    {

    }

    void Update()
    {
        float newX = Mathf.Sin(counter);
        float newZ = Mathf.Cos(counter);
        transform.position = center + new Vector3(offset.x * newX, offset.y, offset.z * newZ);
        transform.LookAt(center, Vector3.up);

        counter += orbitSpeed * .1f * Time.deltaTime;
    }
}
