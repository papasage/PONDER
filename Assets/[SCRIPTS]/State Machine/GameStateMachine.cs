using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GameStateMachine : StateMachine
{
    public static GameStateMachine instance;
    FishingRodSpawner rodSpawner;

    BoidBehavior caughtFish;        //the fish data that was caught
    GameObject caughtFishDisplay;   //the object that parents the trophy
    GameObject trophy;              //the instance of the caughtFish that is displayed
    public bool rodSpawnerReady = false;
    public bool tackleMenuReady = false;
    public bool playerCaughtInputReady = false;

    public int playerGold = 0;

    [SerializeField] GameObject UI_CaughtPrompt;
    [SerializeField] GameObject UI_CaughtCombo;

    //GameStates Scripts made with the IState interface
    //Assets -> Scripts -> GameManager -> States
    public GameIdleState IdleState {get; private set;}
    public GameCastingState CastingState {get; private set;}
    public GameCastedState CastedState {get; private set;}
    public GameBiteState BiteState {get; private set;}
    public GameReelingState ReelingState {get; private set;}
    public GameLandingState LandingState {get; private set;}
    public GameFightingState FightingState {get; private set;}
    public GameScoringState ScoringState {get; private set;}

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        //instantiate states, then connect them to this script
        IdleState = new GameIdleState(this);
        CastingState = new GameCastingState(this);
        CastedState = new GameCastedState(this);
        BiteState = new GameBiteState(this);
        ReelingState = new GameReelingState(this);
        LandingState = new GameLandingState(this);
        FightingState = new GameFightingState(this);
        ScoringState = new GameScoringState(this);

        caughtFishDisplay = GameObject.Find("CaughtFishDisplayObject");
        rodSpawner = GameObject.Find("FishingRodSpawner").GetComponent<FishingRodSpawner>();

        //UIController.instance.SetCaughtMenu(false);
        UI_CaughtPrompt.SetActive(false);

        //Hide Mouse
        Cursor.visible = false;

    }
    private void Start()
    {
        //defined in the StateMachine base class
        //Start in idle mode
        Idle();
    }


    new void Update()
    {
        if (rodSpawnerReady)
        {
            UIController.instance.UpdateButtonSprite("X", UIController.EquipmentIcon.RodSprite);

            //EQUIP THE FISHING ROD
            if (Input.GetButtonDown("X"))
            {
                //Equip Rod
                rodSpawner.SpawnRod();
                rodSpawnerReady = false;
                tackleMenuReady = false;

                UIController.instance.UpdateButtonSprite("X", UIController.EquipmentIcon.RodUsedSprite);
                UIController.instance.UpdateButtonSprite("Y", UIController.EquipmentIcon.nullSprite);

                StartCoroutine(UIController.instance.CastingTipCoroutine());
                PlayerController.instance.playerMovementLocked = true;
            }
        }

        else if (rodSpawner.currentRod != null)
        {
            //UNEQUIP THE FISHING ROD
            if (Input.GetButtonDown("X") && !UIController.instance.equipBlock)
            {
                if (rodSpawner.currentRod.GetComponent<FishingRod>().hookedFish != null)
                {
                    rodSpawner.currentRod.GetComponent<FishingRod>().hookedFish.GetComponent<BoidBehavior>().Unhook();
                }
                rodSpawner.currentRod.GetComponent<FishingRod>().isReeled = true;
                rodSpawner.currentRod.GetComponent<FishingRod>().isCasted = false;
                ControllerInputManager.instance.canReel = false;

                rodSpawner.DespawnRod();
                Idle();
            }

            if (rodSpawner.currentRod.GetComponent<FishingRod>().cameraToggleReady)
            {
                if (Input.GetButtonDown("Y"))
                {
                    rodSpawner.currentRod.GetComponent<FishingRod>().ToggleCamera();
                }
            }
        }

        if (tackleMenuReady)
        {
            //TOGGLE THE TACKLE
            if (Input.GetButtonDown("Y"))
            {
                UIController.instance.ToggleTackleMenu();
            }

            if (UIController.instance.tackleMode == false)
            {
                UIController.instance.UpdateButtonSprite("Y", UIController.EquipmentIcon.TackleSprite);
            }
            else
            {
                UIController.instance.UpdateButtonSprite("Y", UIController.EquipmentIcon.TackleUsedSprite);
                UIController.instance.UpdateButtonSprite("X", UIController.EquipmentIcon.nullSprite);
            }
        }

        if (playerCaughtInputReady)
        {
            //KEEP FISH
            if (Input.GetButtonDown("A"))
            {
                StartCoroutine(FishKeepCoroutine());
            }
            //SELL FISH
            if (Input.GetButtonDown("B"))
            {
                StartCoroutine(FishSellCoroutine());
            }
        }



    }

    public void Idle()
    {
        ChangeState(IdleState);

        AudioManager.instance.AmbienceDock();
        AudioManager.instance.MusicPeaceful();

        rodSpawnerReady = true;
        tackleMenuReady = true;

        PlayerController.instance.playerMovementLocked = false;

        if (trophy != null)
        {
            Destroy(trophy);
        }
    }

    public void Casting()
    {
        ChangeState(CastingState);
        //UIController.instance.ToggleCastTip();
    }

    public void Casted()
    {
        ChangeState(CastedState);
    }

    public void Bite()
    {
        ChangeState(BiteState);
        //AudioManager.instance.MusicAction();
        ChangeState(ReelingState);
    }
    public void Reeling()
    {
        ChangeState(ReelingState);
    }
    public void Landing(BoidBehavior caught)
    {
        UIController.instance.SetButtonHUD(false);
        ChangeState(LandingState);
        caughtFish = caught;
        StartCoroutine(LandingCoroutine());
        
    }
    public void FishCaught()
    {
        ChangeState(FightingState);

        if (trophy != null)
        {
            Destroy(trophy);
        }

        caughtFishDisplay = GameObject.Find("CaughtFishDisplayObject");
        
        trophy = Instantiate(caughtFish.mesh, caughtFishDisplay.transform.position, caughtFishDisplay.transform.rotation, caughtFishDisplay.transform);
        StartCoroutine(caughtFishDisplay.GetComponent<RotateObject>().RevealFishModelCoroutine());

        // AudioManager.instance.MusicFishCaught();

        //UIController.instance.SetCaughtMenu(true);
        UI_CaughtPrompt.SetActive(true);
        UIController.instance.SetGoldHUD(true);

        UIController.instance.ClearOutComboChainMeter();

        GameObject.Find("CaughtData_Breed").GetComponent<TMP_Text>().text = caughtFish.maidenName;
        //GameObject.Find("CaughtData_Size").GetComponent<TMP_Text>().text = caughtFish.sizeMultiplier.ToString("F2");
        GameObject.Find("CaughtData_Level").GetComponent<TMP_Text>().text = caughtFish.foodScore.ToString("F0");
        GameObject.Find("CaughtData_Sketch").GetComponent<Image>().sprite = caughtFish.sketch;

        if(caughtFish.sizeMultiplier < .9f)
        {
            GameObject.Find("CaughtData_Size").GetComponent<TMP_Text>().text = "S";
        }
        if(caughtFish.sizeMultiplier >= .9f && caughtFish.sizeMultiplier < 1.5f)
        {
            GameObject.Find("CaughtData_Size").GetComponent<TMP_Text>().text = "M";
        }
        if(caughtFish.sizeMultiplier >= 1.5f && caughtFish.sizeMultiplier < 2.5f)
        {
            GameObject.Find("CaughtData_Size").GetComponent<TMP_Text>().text = "L";
        }
        if(caughtFish.sizeMultiplier >= 2.5f && caughtFish.sizeMultiplier < 5f)
        {
            GameObject.Find("CaughtData_Size").GetComponent<TMP_Text>().text = "L";
        }
        if(caughtFish.sizeMultiplier > 5f)
        {
            GameObject.Find("CaughtData_Size").GetComponent<TMP_Text>().text = "XL";
        }
        

        if (caughtFish.comboMeter > 1)
        {
            //UIController.instance.SetCaughtComboPopUp(true);
            UI_CaughtCombo.SetActive(true);
            //COMBO DATA
            GameObject.Find("CaughtData_Combo").GetComponent<TMP_Text>().text = "x" + caughtFish.comboMeter.ToString();
        }
        else
        { 
            //UIController.instance.SetCaughtComboPopUp(false);
            UI_CaughtCombo.SetActive(false);
        }


        playerCaughtInputReady = true;

        //StartCoroutine(FightingCoroutine());
    }
    public void Scoring()
    {
        ChangeState(ScoringState);
        
    }
    IEnumerator LandingCoroutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < .9f)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        FishCaught();
    }
    
    IEnumerator FishKeepCoroutine()
    {
        StartCoroutine(caughtFishDisplay.GetComponent<RotateObject>().HideFishModelCoroutine());
        AudioManager.instance.FishSaved();

        //THIS IS WHERE WE APPLY THE LOGIC FOR SAVING THE FISH
        caughtFish.timeCaught = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
        FishDataManager.instance.SaveFishData(caughtFish);

        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        //UIController.instance.SetCaughtMenu(false);
        //UIController.instance.SetCaughtComboPopUp(false);
        UI_CaughtCombo.SetActive(false);
        UI_CaughtPrompt.SetActive(false);
        UIController.instance.SetGoldHUD(false);

        UIController.instance.SetButtonHUD(true);

        playerCaughtInputReady = false;
        rodSpawnerReady = true;
        Idle();
    }
    
    IEnumerator FishSellCoroutine()
    {
        StartCoroutine(caughtFishDisplay.GetComponent<RotateObject>().HideFishModelCoroutine());
        AudioManager.instance.FishSold();
        int fishWorth = Mathf.RoundToInt(caughtFish.foodScore) * Mathf.RoundToInt(caughtFish.sizeMultiplier) * 100;
        playerGold = playerGold + fishWorth;
        UIController.instance.UpdateGoldCount(playerGold);

        //THIS IS WHERE WE APPLY THE LOGIC FOR SELLING THE FISH


        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        //UIController.instance.SetCaughtMenu(false);
        //UIController.instance.SetCaughtComboPopUp(false);
        UI_CaughtCombo.SetActive(false);
        UI_CaughtPrompt.SetActive(false);
        UIController.instance.SetGoldHUD(false);
        UIController.instance.SetButtonHUD(true);

        playerCaughtInputReady = false;
        rodSpawnerReady = true;
        Idle();
    }
}
