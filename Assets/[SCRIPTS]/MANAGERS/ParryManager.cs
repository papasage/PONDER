using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ParryManager : MonoBehaviour
{
    public static ParryManager instance;
    [SerializeField] public GameObject ParryUI;
    public bool parryManagerReady = false; //This is controlled with the PlayableDirector component inside the animation. When this is true, then pressing A will trigger a parry.
    public FishingRod fishingRod;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void PromptParry()
    {
        AudioManager.instance.ParryIncoming();
        fishingRod = GameObject.Find("FishingRod").GetComponent<FishingRod>();
        if (fishingRod != null)
        {
            fishingRod.parryReady = true;
        }

        GetComponent<PlayableDirector>().Play();
    }

    public void MissedParry() //REFERENCED IN THE PARRY TIMELINE USING A SIGNAL ASSET!
    {
        //When the animation window for a parry passes, auto-fail and that will loop it again
        fishingRod = GameObject.Find("FishingRod").GetComponent<FishingRod>();
        if (fishingRod != null)
        {
            fishingRod.ParryFail(0f);
        }

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
