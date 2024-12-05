using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameBiteState;
using static GameCastedState;
using static GameCastingState;
using static GameFightingState;
using static GameIdleState;
using static GameLandingState;
using static GameReelingState;
using static GameScoringState;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    private Camera playerCamera;

    public Transform waterLevel;
    public bool isUnderWater;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        playerCamera = GameObject.FindObjectOfType<Camera>();
        waterLevel = GameObject.Find("WaterSurface").transform;
    }

    private void Update()
    {
        if (playerCamera.transform.position.y < waterLevel.position.y)
        {
            isUnderWater = true;
        }
        else isUnderWater = false;
    }

}
