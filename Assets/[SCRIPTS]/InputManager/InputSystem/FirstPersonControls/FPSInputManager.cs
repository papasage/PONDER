using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSInputManager : MonoBehaviour
{
    private static FPSInputManager instance;

    public static FPSInputManager Instance
    {
        get { return instance; }
    }

    private PlayerControls playerControls;

    private void Awake()
    {
        if (instance == null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else instance = this;

        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    public Vector2 GetPlayerMovement()
    {
        return playerControls.Player.Movement.ReadValue<Vector2>();
    }
    
    public Vector2 GetPlayerLook()
    {
        return playerControls.Player.Look.ReadValue<Vector2>();
    }
    public bool PlayerJumpedThisFrame()
    {
        return playerControls.Player.Jump.triggered;
    }
}
