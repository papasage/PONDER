using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class ControllerInputManager : MonoBehaviour
{
    public static ControllerInputManager instance;

    //public delegate void OnUseEquipment();
    //public static OnUseEquipment onUseEquipment;
    
    //public delegate void OnCancel();
    //public static OnCancel onCancel;

    //public delegate void OnEquip1();
    //public static OnEquip1 onEquip1;
    
    //public delegate void OnEquip2();
    //public static OnEquip2 onEquip2;

    public delegate void OnReel();
    public static OnReel onReel;

    [SerializeField] public float joystickDeadzone = 0.5f;
    [SerializeField] float reelingSensitivity = 0.5f;
    public float leftStickAngle;

    //reel motion vaiables
    private Vector2 lastLeftStickValue = Vector2.zero;
    public bool isReeling;
    public bool canReel;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Update()
    {
        LeftStick();
        RightStick();
        
        AButton();
        BButton();
        XButton();
        YButton();

        StartButton();
        SelectButton();

        LeftTrigger();
        RightTrigger();
        
        if (canReel && !UIController.instance.reelBlock)
        {
            Reeling();
        }
    }

    void LeftStick()
    {
        if (Input.GetAxisRaw("LeftStickX") < -joystickDeadzone)
        {
        }

        if (Input.GetAxisRaw("LeftStickX") > joystickDeadzone)
        {
        }

        if (Input.GetAxisRaw("LeftStickY") < -joystickDeadzone)
        {
        }

        if (Input.GetAxisRaw("LeftStickY") > joystickDeadzone)
        {
        }
    }
    void RightStick()
    {
        if (Input.GetAxisRaw("RightStickX") < -joystickDeadzone)
        { 
        }

        if (Input.GetAxisRaw("RightStickX") > joystickDeadzone)
        {  
        }

        if (Input.GetAxisRaw("RightStickY") < -joystickDeadzone)
        {
        }

        if (Input.GetAxisRaw("RightStickY") > joystickDeadzone)
        { 
        }
    }
    void AButton()
    {
        if (Input.GetButtonDown("A"))
        {
            //onUseEquipment();
        }
    }
    void BButton()
    {
        if (Input.GetButtonDown("B"))
        {
            //onCancel();
        }
    }
    void XButton()
    {
        if (Input.GetButtonDown("X"))
        {
           // onEquip1();
        }
    }
    void YButton()
    {
        if (Input.GetButtonDown("Y"))
        {
            //onEquip2();
        }
    }
    void StartButton()
    {
        if (Input.GetButtonDown("Start"))
        {
            //toggle debug mode
            //UIController.instance.ToggleDiaryMenu();
        }
    }
    void SelectButton()
    {
        if (Input.GetButtonDown("Select"))
        {
            //toggle debug mode
            UIController.instance.ToggleDebugMenu();

        }
    }

    void LeftTrigger()
    {
        //Debug.Log("LT: " + Input.GetAxisRaw("LT"));
    }
    void RightTrigger()
    {
        //Debug.Log("RT: " + Input.GetAxisRaw("RT"));
    }

    void Reeling()
    {
        // Check the left stick input
        Vector2 leftStickValue = new Vector2(Input.GetAxis("LeftStickX"), Input.GetAxis("LeftStickY"));

        // Check if there is any change in left stick input beyond the reelingSensitivity setting.
        if (leftStickValue.magnitude > reelingSensitivity && Vector2.Distance(leftStickValue, lastLeftStickValue) > reelingSensitivity)
        {
            // Trigger reeling
            isReeling = true;

            // leftStickAngle is public and this script is a singleton. pull directly from here to rotate the reel sprite
            leftStickAngle = Mathf.Atan2(leftStickValue.y, leftStickValue.x) * Mathf.Rad2Deg;
            //Debug.Log(leftStickAngle);
        }
        else
        {
            // Stop reeling
            isReeling = false;
        }

        // Save the current left stick value for the next frame
        lastLeftStickValue = leftStickValue;

        if (isReeling)
        {
            //Debug.Log("Reeling Motion Detected!");
            onReel();
        }
    }
}
