using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ParryManager : MonoBehaviour
{
    public static ParryManager instance;
    [SerializeField] public GameObject ParryUI;
    public bool parryManagerReady = false;
    public FishingRod fishingRod;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        //ParryUI = GameObject.Find("ParryTimer");
    }

    public void PromptParry()
    {
        AudioManager.instance.ParryIncoming();
        fishingRod = GameObject.Find("FishingRod").GetComponent<FishingRod>();
        fishingRod.parryReady = true;
        GetComponent<PlayableDirector>().Play();
    }
    
    public void StopParry()
    {
        GetComponent<PlayableDirector>().Stop();
        ParryUI.SetActive(false);
    }

    public void LoopParry()
    {
        if (fishingRod != null)
        {
            StartCoroutine(fishingRod.ParryCountdown());
        }
        
    }
}
