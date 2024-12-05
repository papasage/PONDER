using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    PlayerSpawner playerSpawner;
    public Transform waterLevel;

    private CharacterController controller;
    private Vector3 playerVelocity;
    [SerializeField] public bool groundedPlayer;
    private Transform cameraTransform;
    private Cinemachine.CinemachineVirtualCamera virtualCamera;
    private float playerRotationY;

    public bool playerMovementLocked = false;

    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;

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

        playerSpawner = FindObjectOfType<PlayerSpawner>();
        waterLevel = GameObject.Find("WaterSurface").transform;
    }

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
        //virtualCamera = transform.Find("Virtual Camera");
    }

    void Update()
    {
        GroundCheck();
        ApplyGravity();
        PlayerXRotation();

        //if (transform.position.y < waterLevel.position.y - 5)
        //{
            //playerSpawner.Respawn();
        //}

        if (playerMovementLocked == false)
        {
            PlayerMovement();

            if (Input.GetButtonDown("A") && groundedPlayer)
            {
                PlayerJump();
            }
        }

    }
    void GroundCheck()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
    }
    void PlayerMovement()
    {
        Vector3 move = new Vector3(Input.GetAxis("LeftStickX"), 0, Input.GetAxis("LeftStickY"));
        move = cameraTransform.forward * move.z + cameraTransform.right * move.x;
        move.y = 0f;
        controller.Move(move * Time.deltaTime * playerSpeed);
    }
    void PlayerXRotation()
    {
        if (virtualCamera != null)
        {
            // Get the rotation of the virtual camera
            Quaternion cameraRotation = virtualCamera.transform.rotation;

            // Convert rotation to Euler angles
            Vector3 cameraEulerAngles = cameraRotation.eulerAngles;

            // Ignore x and z rotation (pitch and roll)
            cameraEulerAngles.x = 0f;
            cameraEulerAngles.z = 0f;

            // Apply camera's Y rotation to player's Y rotation
            playerRotationY += cameraEulerAngles.y;

            // Apply rotation to the player gameObject
            transform.rotation = Quaternion.Euler(0f, playerRotationY, 0f);
        }
    }

    void PlayerJump()
    {
        playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
    }

    void ApplyGravity()
    {
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}
