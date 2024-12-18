using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadLookAt : MonoBehaviour
{
    public GameObject headBone;
    Quaternion startingRotation;
    bool isLooking;
    GameObject lookTarget;

    void Start()
    {
        startingRotation = headBone.transform.rotation;
    }

    void LateUpdate()
    {
        if (isLooking)
        {
            headBone.transform.LookAt(lookTarget.transform.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger has a CharacterController component
        CharacterController characterController = other.GetComponent<CharacterController>();

        Debug.Log("Collision");

        // If a CharacterController component is found, log a message
        if (characterController != null)
        {
            Debug.Log("Player Found");
            isLooking = true;
            lookTarget = other.gameObject;
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
        }
    }
}

