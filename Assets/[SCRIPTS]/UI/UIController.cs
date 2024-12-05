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
    [SerializeField] TMP_Text Xtext;
    [SerializeField] TMP_Text Ytext;
    [SerializeField] Image Xicon;
    [SerializeField] Image Yicon;
    [SerializeField] Sprite RodSprite;
    [SerializeField] Sprite RodUsedSprite;
    [SerializeField] Sprite TackleSprite;
    [SerializeField] Sprite TackleUsedSprite;
    [SerializeField] Sprite CameraSprite;
    [SerializeField] Sprite nullSprite;

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
    [SerializeField] GameObject UI_ReelBarFishIcon;

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
    [SerializeField] TMP_Text UIStateText;
    [SerializeField] Image UIStateSprite;

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
        // Initialize UIController
        Initialize();

        UI_ReelBarFishIcon = GameObject.Find("ReelBarFishIcon");
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

        if (currentRod != null)
        {
            //CalculateLineLength();
            if (currentRod.RTBLineSnapped || currentRod.BTHLineSnapped)
            {
                if (UI_ReelBarFishIcon != null)
                {
                    SetProgressBarFishIcon(false);
                }
                    
            }
        }
        else
        {
            if (UI_ReelBarFishIcon != null)
            {
                SetProgressBarFishIcon(false);
            }

            lineDistance.value = 0f;
        }

    }

    private void Initialize()
    {
        // Initialize UIController here
        UI_ReelBarFishIcon = GameObject.Find("ReelBarFishIcon");
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
        UIStateText.text = "Idle";
    }
    void UICasting()
    {
        UIStateText.text = "Casting";
    }
    void UICasted()
    {
        UIStateText.text = "Casted";
        
    }
    void UIBite()
    {
        UIStateText.text = "Bite";
        SetProgressBarFishIcon(true);

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
        UIStateText.text = "Reeling";
    }
    void UILanding()
    {
        UIStateText.text = "Landing";
        SetProgressBarFishIcon(false);
        //ToggleReelProgressBar();
    }
    void UIFighting()
    {
        UIStateText.text = "Caught!";
    }
    void UIScoring()
    {
        UIStateText.text = "Shop";
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
            Debug.Log("LINE HEALTH: " + healthPercentage);

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

    public void UpdateYButtonText(string text)
    {
        if(text == "CHANGE CAMERA")
        {
            Yicon.sprite = CameraSprite;
        }
        if(text == "OPEN TACKLE")
        {
            Yicon.sprite = TackleSprite;
        }
        if(text == "CLOSE TACKLE")
        {
            Yicon.sprite = TackleUsedSprite;
        }
        if(text == " ")
        {
            Yicon.sprite = nullSprite;
        }
    }
    
    public void UpdateXButtonText(string text)
    {
        if(text == "EQUIP ROD")
        {
            Xicon.sprite = RodSprite;
        }
        if (text == "UNEQUIP ROD")
        {
            Xicon.sprite = RodUsedSprite;
        }
        if (text == " ")
        {
            Xicon.sprite = nullSprite;
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
    
    public void SetProgressBarFishIcon(bool status)
    {
        if (UI_ReelBarFishIcon != null)
        {
            UI_ReelBarFishIcon.SetActive(status);
        }
        else Debug.Log("UI_ReelBarFishIcon not found.");
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
        UIController.instance.ToggleDiaryMenu();

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
    public void SetCaughtMenu(bool status)
    {
        UI_TackleWindow.SetActive(caughtMode);
    }
    public void SetCaughtComboPopUp(bool status)
    {
        UI_TackleWindow.SetActive(status);
    }

    public void ToggleCastTip()
    {
        castTipMode = !castTipMode;
        UI_CastPrompt.SetActive(castTipMode);
    }

    public void ComboFlash()
    {
        GetComponent<PlayableDirector>().Play();
    }

    IEnumerator ReelingTipCoroutine()
    {
        if (!reelTipOccured)
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
    IEnumerator LineHealthTipCoroutine()
    {
        if (!lineHealthTipOccured)
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
    public IEnumerator CastingTipCoroutine()
    {
        if (!castTipOccured)
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
    public IEnumerator EquipTipCoroutine()
    {
        if (!equipTipOccured)
        {
            UI_EquipPrompt.SetActive(true);
            equipTipOccured = true;
            AudioManager.instance.UIPopup();
            yield return new WaitUntil(() => Input.GetButtonDown("X"));
            UI_EquipPrompt.SetActive(false);
        }
    } 
    public IEnumerator ParryTipCoroutine()
    {
        if (!parryTipOccured)
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
