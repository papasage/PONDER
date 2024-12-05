using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{
    FishingRodSpawner rodSpawner;
    FishingRod currentRod;
    private Rigidbody rb;
    SphereCollider sphereCollider;
    private Terrain terrain; // Reference to the terrain
    private float waterLevel;
    public float raycastDistance = 1f; // Distance of the raycast
    public float depthBeforeSubmreged = 1f;
    public float displacementAmount = 3f;

    public bool isFloating;

    private void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
        rb = GetComponent<Rigidbody>();
        rodSpawner = GameObject.Find("FishingRodSpawner").GetComponent<FishingRodSpawner>(); 
        waterLevel = GameObject.Find("WaterLevel").transform.position.y;

    }

    void Start()
    {
        // Get the Terrain component
        terrain = FindObjectOfType<Terrain>();
        currentRod = GameObject.Find("FishingRod").GetComponent<FishingRod>();
    }

    void Update()
    {

        if (currentRod.isCasting == true)
        {
            // Perform a raycast downwards from the sphere's position

            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance))
            {
                // Check if the ray hit the terrain collider
                if (hit.collider != null && hit.collider.CompareTag("Terrain"))
                {
                    Debug.Log("Sphere collided with terrain!");
                    GameStateMachine.instance.rodSpawnerReady = true;
                    GameStateMachine.instance.Idle();
                    rodSpawner.DespawnRod();
                    UIController.instance.SetReelProgressBar(false);
                }
            }
        }
        

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
