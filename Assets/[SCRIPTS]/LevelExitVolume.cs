using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExitVolume : MonoBehaviour
{
    PlayerSpawner playerSpawner;

    private void Awake()
    {
        playerSpawner = FindObjectOfType<PlayerSpawner>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger has a CharacterController component
        CharacterController characterController = other.GetComponent<CharacterController>();

        // If a CharacterController component is found, log a message
        if (characterController != null)
        {
            SceneManager.LoadScene("LevelSelect");
        }
    }
}
