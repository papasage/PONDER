using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
//using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    public static UIController instance;
    [SerializeField] FishingRod currentRod;

    [Header("Button UI")]
    [SerializeField] GameObject ButtonHUD;

    [SerializeField] Image Xicon; //button icon
    [SerializeField] Image Yicon; //button icon
    [SerializeField] Image Aicon; //button icon

    [SerializeField] Sprite RodSprite;
    [SerializeField] Sprite RodUsedSprite;
    [SerializeField] Sprite TackleSprite;
    [SerializeField] Sprite TackleUsedSprite;
    [SerializeField] Sprite CameraSprite;
    [SerializeField] Sprite nullSprite;

    public enum EquipmentIcon
    {
        RodSprite,
        RodUsedSprite,
        TackleSprite,
        TackleUsedSprite,
        CameraSprite,
        nullSprite
    }

    [Header("Fishing Line Tension Data")]
    public bool rodIsEquipped;
    [SerializeField] SpringJoint RodToBobber;
    [SerializeField] SpringJoint BobberToHook;
    [SerializeField] TMP_Text RTBForce;
    [SerializeField] TMP_Text BTHForce;
    [SerializeField] TMP_Text RTBDamage;
    [SerializeField] TMP_Text BTHDamage;
    [SerializeField] TMP_Text RTBLength;
    [SerializeField] TMP_Text BTHLength;

    [Header("Reel Progress Bar")]
    public bool reelBarMode = false;
    [SerializeField] public GameObject ProgressBarParent;
    [SerializeField] public Slider lineDistance;
    static float t = 0.0f;
    [SerializeField] Image progressBarFill;
    [SerializeField] Color lerpedColor;
    [SerializeField] public Color colorHealthMax;
    [SerializeField] public Color colorHealthDepleated;
    [SerializeField] GameObject FishComboStack;
    private List<GameObject> fishStackList = new List<GameObject>();

    [Header("Cast Charging Bar")]
    [SerializeField] public GameObject ChargeBarWindow;
    [SerializeField] public GameObject ChargeBarParent;
    [SerializeField] public Slider castCharge;
    [SerializeField] Image chargeBarFill;

    [Header("Debug Menu")]
    public bool debugMode = false;
    [SerializeField] GameObject debugMenu;
    [SerializeField] TMP_Text UIBoidCountText;
    private List<GameObject> boids;

    [Header("Journal Menu")]
    public bool diaryMode = false;
    [SerializeField] GameObject diaryMenu;

    [Header("Tackle Box Menu")]
    public bool tackleMode = false;
    [SerializeField] GameObject UI_TackleWindow;

    [Header("Player Gold")]
    [SerializeField] GameObject UI_GoldWindow;
    [SerializeField] TMP_Text UIGoldCountText;

    [Header("Caught Menu")]
    [SerializeField] GameObject UI_CaughtPrompt;
    public bool caughtMode = false;

    [Header("Caught Menu Combo Pop-up")]
    [SerializeField] GameObject UI_CaughtCombo;
    public bool comboMode = false;

    [Header("Gameplay Tips")]
    public bool equipBlock = false;
    public bool reelBlock = false;
    public bool castTipMode = false;
    public bool gameplayTipsEnabled = false;

    public bool castTipOccured = false;
    [SerializeField] GameObject UI_CastPrompt;
    public bool reelTipOccured = false;
    [SerializeField] GameObject UI_ReelPrompt;
    public bool parryTipOccured = false;
    [SerializeField] GameObject UI_ParryPrompt;
    public bool lineHealthTipOccured = false;
    [SerializeField] GameObject UI_LineHealthPrompt;
    public bool equipTipOccured = false;
    [SerializeField] GameObject UI_EquipPrompt;

    private void OnEnable()
    {
        SubscribeToEvents();
    }
    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }
    private void SubscribeToEvents()
    {
        GameIdleState.onStateIdle += UIIdle;
        GameCastingState.onStateCasting += UICasting;
        GameCastedState.onStateCasted += UICasted;
        GameBiteState.onStateBite += UIBite;
        GameReelingState.onStateReeling += UIReeling;
        GameLandingState.onStateLanding += UILanding;
        GameFightingState.onStateFighting += UIFighting;
        GameScoringState.onStateScoring += UIScoring;

        FishSpawner.onSpawn += UpdateBoidCount;
        BoidBehavior.onDeath += UpdateBoidCount;
    }
    private void UnsubscribeFromEvents()
    {
        GameIdleState.onStateIdle -= UIIdle;
        GameCastingState.onStateCasting -= UICasting;
        GameCastedState.onStateCasted -= UICasted;
        GameBiteState.onStateBite -= UIBite;
        GameReelingState.onStateReeling -= UIReeling;
        GameLandingState.onStateLanding -= UILanding;
        GameFightingState.onStateFighting -= UIFighting;
        GameScoringState.onStateScoring -= UIScoring;

        FishSpawner.onSpawn -= UpdateBoidCount;
        BoidBehavior.onDeath -= UpdateBoidCount;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
        UnsubscribeFromEvents();
        SubscribeToEvents();
    }
    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            // Destroy duplicate instance
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        UpdateBoidCount();
    }
    private void Update()
    {
        //CalculateLineData();

        if (reelBarMode)
        {
            ProgressBarColor();

            lineDistance.value = Mathf.Lerp(lineDistance.value,RodToBobber.maxDistance,t);
            t += 0.02f * Time.deltaTime;
        }
        else
        {
            lineDistance.value = 0f;
        }

    }
    void UpdateBoidCount()
    {
        //boids = new List<GameObject>(GameObject.FindObjectsOfType<BoidBehavior>().Select(boid => boid.gameObject));

        List<BoidBehavior> boids = PondManager.Instance.GetAllFish();

        int UICount = 0;
        foreach (BoidBehavior boid in boids)
        {   
            if (boid != null)
            {
                if (boid.isDead == false)
                {
                    UICount++;
                }
            }
        }
        //Debug.Log("Boids Initialized! Found Boids: " + boids.Count);
        UIBoidCountText.text = UICount.ToString();
    }
    public void UpdateGoldCount(int amount)
    {
        UIGoldCountText.text = amount.ToString();
    }
    void UIIdle()
    {
    }
    void UICasting()
    {
    }
    void UICasted()
    {  
    }
    void UIBite()
    {
        // Check if the UIController instance is null
        if (instance != null)
        {
            // Start the coroutine only if the instance is not null
            StartCoroutine(instance.ReelingTipCoroutine());
        }
        else
        {
            // Log a warning if the instance is null
            Debug.LogWarning("UIController instance is null. Unable to start coroutine.");
        }
    }
    void UIReeling()
    {
    }
    void UILanding()
    {
    }
    void UIFighting()
    {
    }
    void UIScoring()
    {
    }
    void CalculateLineData()
    {
        if (rodIsEquipped)
        {
            if (currentRod.RTBLineSnapped)
            {
                RTBDamage.color = Color.red;
                RTBDamage.text = "BREAK";
                return;
            }

            if (currentRod.BTHLineSnapped)
            {
                BTHDamage.color = Color.red;
                BTHDamage.text = "BREAK";
                return;
            }

            RTBForce.text = Mathf.Round(RodToBobber.currentForce.magnitude).ToString();
            BTHForce.text = Mathf.Round(BobberToHook.currentForce.magnitude).ToString();

            if (RodToBobber.currentForce.magnitude > currentRod.maxLineTension)
            {
                RTBForce.color = Color.red;
            }
            else RTBForce.color = Color.white;

            if (BobberToHook.currentForce.magnitude > currentRod.maxLineTension)
            {
                BTHForce.color = Color.red;
            }
            else BTHForce.color = Color.white;

            

            if (currentRod != null)
            {
                RTBDamage.color = Color.white;
                RTBDamage.text = currentRod.rodToBobberLineHealth.ToString();
                BTHDamage.color = Color.white;
                BTHDamage.text = currentRod.bobberToHookLineHealth.ToString();
            }
            
        }
        else
        {
            RTBForce.text = "-";
            BTHForce.text = "-";
            RTBDamage.text = "-";
            BTHDamage.text = "-";
        }
    }
    void CalculateLineLength()
    {
        if (currentRod.rodToBobberString != null)
        {
            RTBLength.text = Mathf.Round(currentRod.rodToBobberString.maxDistance).ToString();
        }

        if (currentRod.bobberToHookString != null)
        {
            BTHLength.text = Mathf.Round(currentRod.bobberToHookString.maxDistance).ToString();
        }
    }
    public void InitializeRodUI(float lineSlack)
    {
        RodToBobber = GameObject.Find("Rod").GetComponent<SpringJoint>();
        BobberToHook = GameObject.Find("Bobber").GetComponent<SpringJoint>();

        lineDistance.maxValue = lineSlack;

        currentRod = GameObject.Find("FishingRod").GetComponent<FishingRod>();
    }
    void ProgressBarColor()
    {
        if (rodIsEquipped && currentRod != null)
        {
            float BTHHealthPercentage = currentRod.bobberToHookLineHealth / currentRod.lineMaxHealth;
            float RTBHealthPercentage = currentRod.rodToBobberLineHealth / currentRod.lineMaxHealth;

            float healthPercentage = Mathf.Min(BTHHealthPercentage, RTBHealthPercentage);

            // Ensure the percentage is between 0 and 1
            healthPercentage = Mathf.Clamp01(healthPercentage);
            //Debug.Log("LINE HEALTH: " + healthPercentage);

            if(healthPercentage < .25f && currentRod.isCasted)
            {
                StartCoroutine(LineHealthTipCoroutine());
            }

            // Lerp the color based on the percentage
            lerpedColor = Color.Lerp(colorHealthDepleated, colorHealthMax, healthPercentage);

            // Assign the lerped color to the progressBarFill
            progressBarFill.color = lerpedColor;

            if (currentRod.RTBLineSnapped || currentRod.BTHLineSnapped)
            {
                progressBarFill.color = Color.black;
            }
        }
    }
    
    public void SetButtonHUD(bool status)
    {
        ButtonHUD.SetActive(status);
    }
    public void SetGoldHUD(bool status)
    {
        UI_GoldWindow.SetActive(status);
    }

    public void UpdateButtonSprite(string button, EquipmentIcon sprite)
    {
        if (button == "X")
        {
            switch (sprite)
            {
                case EquipmentIcon.RodSprite:
                    Xicon.sprite = RodSprite;
                    break;
                case EquipmentIcon.RodUsedSprite:
                    Xicon.sprite = RodUsedSprite;
                    break;
                case EquipmentIcon.TackleSprite:
                    Xicon.sprite = TackleSprite;
                    break;
                case EquipmentIcon.TackleUsedSprite:
                    Xicon.sprite = TackleUsedSprite;
                    break;
                case EquipmentIcon.CameraSprite:
                    Xicon.sprite = CameraSprite;
                    break;
                case EquipmentIcon.nullSprite:
                    Xicon.sprite = nullSprite;
                    break;
            }
        }
        if (button == "Y")
        {
            switch (sprite)
            {
                case EquipmentIcon.RodSprite:
                    Yicon.sprite = RodSprite;
                    break;
                case EquipmentIcon.RodUsedSprite:
                    Yicon.sprite = RodUsedSprite;
                    break;
                case EquipmentIcon.TackleSprite:
                    Yicon.sprite = TackleSprite;
                    break;
                case EquipmentIcon.TackleUsedSprite:
                    Yicon.sprite = TackleUsedSprite;
                    break;
                case EquipmentIcon.CameraSprite:
                    Yicon.sprite = CameraSprite;
                    break;
                case EquipmentIcon.nullSprite:
                    Yicon.sprite = nullSprite;
                    break;
            }
        }
        if (button == "A")
        {
            switch (sprite)
            {
                case EquipmentIcon.RodSprite:
                    Aicon.sprite = RodSprite;
                    break;
                case EquipmentIcon.RodUsedSprite:
                    Aicon.sprite = RodUsedSprite;
                    break;
                case EquipmentIcon.TackleSprite:
                    Aicon.sprite = TackleSprite;
                    break;
                case EquipmentIcon.TackleUsedSprite:
                    Aicon.sprite = TackleUsedSprite;
                    break;
                case EquipmentIcon.CameraSprite:
                    Aicon.sprite = CameraSprite;
                    break;
                case EquipmentIcon.nullSprite:
                    Aicon.sprite = nullSprite;
                    break;
            }
        }
    }

    public void SetReelProgressBar(bool status)
    {
        reelBarMode = status;
        ProgressBarParent.SetActive(status);
    }

    public void SetCastWindow(bool status)
    {
        ChargeBarWindow.SetActive(status);
    }

    public void ToggleDebugMenu()
    {
        debugMode = !debugMode;
        debugMenu.SetActive(debugMode);
    }
    
    public void ToggleDiaryMenu()
    {
        diaryMode = !diaryMode;
        diaryMenu.SetActive(diaryMode);

        if (diaryMode)
        {
            PlayerController.instance.playerMovementLocked = true;
            GameStateMachine.instance.rodSpawnerReady = false;
        }
        else
        {
            PlayerController.instance.playerMovementLocked = false;
            GameStateMachine.instance.rodSpawnerReady = true;
        }
    }
    public void ToggleTackleMenu()
    {
        tackleMode = !tackleMode;
        UI_TackleWindow.SetActive(tackleMode);

        SetGoldHUD(tackleMode);
        ToggleDiaryMenu();

        if (tackleMode)
        {
            PlayerController.instance.playerMovementLocked = true;
            GameStateMachine.instance.rodSpawnerReady = false;
        }
        else
        {
            PlayerController.instance.playerMovementLocked = false;
            GameStateMachine.instance.rodSpawnerReady = true;
        }
    }


    public void ComboFlash()
    {
        GetComponent<PlayableDirector>().Play();
    }

    public void ComboChainMeter(Sprite icon)
    {
        GameObject fishComboStackSprite = new GameObject("fishComboStackSprite", typeof(Image));
        fishComboStackSprite.transform.SetParent(FishComboStack.transform, false);
        fishComboStackSprite.GetComponent<Image>().sprite = icon;
        fishComboStackSprite.transform.localScale = new Vector3(2, 2, 2);
        fishStackList.Add(fishComboStackSprite);
    }

    public void ClearOutComboChainMeter()
    {
        // Iterate through the list and destroy each game object
        foreach (GameObject fishComboSprite in fishStackList)
        {
            Destroy(fishComboSprite);
        }

        // Clear the list
        fishStackList.Clear();
    }

    //Triggered in UIController (this) during UIBite();
    IEnumerator ReelingTipCoroutine()
    {
        if (!reelTipOccured && gameplayTipsEnabled)
        {
            equipBlock = true;
            yield return new WaitForSecondsRealtime(.5f);
            UI_ReelPrompt.SetActive(true);
            reelTipOccured = true;
            AudioManager.instance.UIPopup();
            PauseManager.instance.Pause();
            yield return new WaitUntil(() => ControllerInputManager.instance.isReeling);
            PauseManager.instance.Unpause();

            UI_ReelPrompt.SetActive(false);
            equipBlock = false;
        }
    }
    // Triggered in UIController (this) when the line health drops below 25%
    IEnumerator LineHealthTipCoroutine()
    {
        if (!lineHealthTipOccured && gameplayTipsEnabled)
        {
            UI_LineHealthPrompt.SetActive(true);
            lineHealthTipOccured = true;
            AudioManager.instance.UIPopup();
            equipBlock = true;
            reelBlock = true;

            //a very hacky solution to account for the line health tip coroutine getting unpaused by other tips, like the parry tip
            PauseManager.instance.Pause();
            yield return new WaitForSecondsRealtime(1f);
            PauseManager.instance.Pause();
            yield return new WaitForSecondsRealtime(1f);
            PauseManager.instance.Pause();
            yield return new WaitForSecondsRealtime(1f);
            PauseManager.instance.Pause();
            yield return new WaitForSecondsRealtime(1f);

            PauseManager.instance.Unpause();

            UI_LineHealthPrompt.SetActive(false);
            equipBlock = false;
            reelBlock = false;
        }
    }

    // Triggered in GameStateMachine when the fishing rod is equipped
    public IEnumerator CastingTipCoroutine()
    {
        if (!castTipOccured && gameplayTipsEnabled)
        {
            equipBlock = true;
            reelBlock = true;
            yield return new WaitForSecondsRealtime(.5f);
            UI_CastPrompt.SetActive(true);
            castTipOccured = true;
            AudioManager.instance.UIPopup();
            PauseManager.instance.Pause();
            yield return new WaitUntil(() => Input.GetAxisRaw("RT") > 0.01);
            PauseManager.instance.Unpause();

            UI_CastPrompt.SetActive(false);
            equipBlock = false;
            reelBlock = false;
        }
    }

    //Triggered by a volume on the dock
    public IEnumerator EquipTipCoroutine()
    {
        if (!equipTipOccured && gameplayTipsEnabled)
        {
            UI_EquipPrompt.SetActive(true);
            equipTipOccured = true;
            AudioManager.instance.UIPopup();
            yield return new WaitUntil(() => Input.GetButtonDown("X"));
            UI_EquipPrompt.SetActive(false);
        }
    } 

    //Triggered when the FishingRod starts the Parry
    public IEnumerator ParryTipCoroutine()
    {
        if (!parryTipOccured && gameplayTipsEnabled)
        {
            UI_ParryPrompt.SetActive(true);
            parryTipOccured = true;
            equipBlock = true;
            reelBlock = true;
            AudioManager.instance.UIPopup();
            PauseManager.instance.Pause();
            yield return new WaitUntil(() => Input.GetButtonDown("A"));
            PauseManager.instance.Unpause();

            UI_ParryPrompt.SetActive(false);
            equipBlock = false;
            reelBlock = false;
        }
    }
}
