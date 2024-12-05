using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] GameObject PlayerPrefab;
    [SerializeField] GameObject currentPlayer;
    Transform Art;

    private void Awake()
    {
        Art = transform.Find("Art");
        Art.gameObject.SetActive(false);
        currentPlayer = Instantiate(PlayerPrefab, transform.position, transform.rotation);
    }

    public void Respawn()
    {
        GameObject.Destroy(currentPlayer);
        GameStateMachine.instance.rodSpawnerReady = true;
        GameStateMachine.instance.Idle();
        FishingRodSpawner.instance.DespawnRod();
        currentPlayer = Instantiate(PlayerPrefab, transform.position, transform.rotation);
    }
}
