using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDespawner : MonoBehaviour
{
    [SerializeField] PlayerSpawner playerSpawner;
    BoxCollider respawnCollider;

    void Awake()
    {
        playerSpawner = FindObjectOfType<PlayerSpawner>();
        respawnCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger has a CharacterController component
        CharacterController characterController = other.GetComponent<CharacterController>();

        // If a CharacterController component is found, log a message
        if (characterController != null)
        {
            playerSpawner.Respawn();
        }
    }
}
