using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class HeadLookAt : MonoBehaviour
{
    bool isLooking;
    Vector3 startPosition;
    Vector3 targetPosition;
    GameObject targetObject;
    [SerializeField] GameObject lookTarget; //This is an object in the prefab hiarchy that controls where the character looks. 
    [SerializeField] Rig lookRig;
    [SerializeField] float trackingSpeed;
    void Start()
    {
        startPosition = lookTarget.transform.position;
    }

    void Update()
    {
        if(targetObject != null)
        {
            targetPosition = targetObject.transform.position;
        }

        if (isLooking && lookTarget.transform.position != targetPosition)
        {
            lookTarget.transform.position = Vector3.Lerp(lookTarget.transform.position, targetPosition, Time.deltaTime * trackingSpeed);
        }
        if (!isLooking && lookTarget.transform.position != startPosition)
        {
            lookTarget.transform.position = Vector3.Lerp(lookTarget.transform.position, startPosition, Time.deltaTime * trackingSpeed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Transform target = other.transform.Find("PlayerCameraSocket");
            targetObject = target.gameObject;
            isLooking = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Check if the object entering the trigger has a CharacterController component
        CharacterController characterController = other.GetComponent<CharacterController>();

        // If a CharacterController component is found, log a message
        if (characterController != null)
        {
            Debug.Log("Player Lost");
            isLooking = false;
            targetObject = null;
        }
    }
}

