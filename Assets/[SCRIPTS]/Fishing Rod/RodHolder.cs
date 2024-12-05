using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RodHolder : MonoBehaviour
{
    Transform playerSocket;

    private void Awake()
    {
        playerSocket = GameObject.Find("RodSpawnLocation").transform;
    }

    private void Update()
    {
        if (playerSocket != null)
        {
            // Set the position of this gameObject to playerSocket's position
            this.transform.position = playerSocket.position;
            this.transform.rotation = playerSocket.rotation;
        }
        else
        {
            Debug.LogError("Player socket is not assigned.");
        };
    }
}
