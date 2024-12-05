using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buoyancy : MonoBehaviour
{
    public float depthBeforeSubmreged = 1.1f;
    public float displacementAmount = 3f;
    private float waterLevel;
    public bool isFloating;
    private Rigidbody rb;

    private void Awake()
    {  
        rb = GetComponent<Rigidbody>();
        waterLevel = GameObject.Find("WaterLevel").transform.position.y;
    }
    private void FixedUpdate()
    {
        if (transform.position.y < waterLevel)
        {
            isFloating = true;
            float displacementMultiplier = Mathf.Clamp01(waterLevel - transform.position.y / depthBeforeSubmreged) * displacementAmount;
            rb.AddForce(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), ForceMode.Acceleration);
        }
        else isFloating = false;
    }

}
