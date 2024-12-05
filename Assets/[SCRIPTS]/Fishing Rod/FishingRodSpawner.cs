using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class FishingRodSpawner : MonoBehaviour
{
    public static FishingRodSpawner instance;

    [SerializeField] GameObject FishingRod;
    [SerializeField] public GameObject currentRod;
    [SerializeField] GameObject spawnLocation;

    private void OnEnable()
    {
        GameLandingState.onStateLanding += DespawnRod;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        else if (instance != null)
        {
            Destroy(instance);
            instance = this;
        }
    }

        void Start()
    {
        
        UIController.instance.rodIsEquipped = false;
    }

    public void SpawnRod()
    {
        //Debug.Log("Spawning Rod");
        if (currentRod != null)
        {
            DespawnRod();
        }
        spawnLocation = GameObject.Find("RodSpawnLocation");
        currentRod = Instantiate(FishingRod, spawnLocation.transform.position, spawnLocation.transform.rotation);
        currentRod.name = "FishingRod";

        //tell the UI that it is okay to look for references to the rod
        UIController.instance.rodIsEquipped = true;
        UIController.instance.SetCastWindow(true);
        UIController.instance.InitializeRodUI(currentRod.GetComponent<FishingRod>().rodToBobberStringSlack);
        AudioManager.instance.RodEquip();
    }

    public void DespawnRod()
    {
        UIController.instance.rodIsEquipped = false;
        UIController.instance.SetReelProgressBar(false);
        UIController.instance.SetCastWindow(false);

        if (currentRod != null)
        {
            //Debug.Log("Despawning Rod");
            StopCoroutine(currentRod.GetComponent<FishingRod>().ParryCountdown());
            ParryManager.instance.StopParry();
            Destroy(currentRod);
            
        }
    }

    public void SnapTimeline()
    {
        GetComponent<PlayableDirector>().Play();
    }
}
