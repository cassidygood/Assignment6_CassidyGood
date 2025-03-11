using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class WalkingScript : MonoBehaviour
{
    public float WalkSpeed = 5f;
    public float SprintMultiplier = 2f;
    public float JumpForce = 5f;
    public float GroundCheckDistance = 0.4f;
    public float LookSensitivityX = 1f;
    public float LookSensitivityY = 1f;
    public float MinYLookAngle = -90f;
    public float MaxYLookAngle = 90f;
    public Transform PlayerCamera;
    public float Gravity = -9.81f;

    private Vector3 velocity;
    private float verticalRotation = 0;
    private CharacterController characterController;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        HandleMovement();
        HandleCameraRotation();
    }

    private void HandleMovement()
    {
        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");

        Vector3 moveDirection = transform.forward * verticalMovement + transform.right * horizontalMovement;
        moveDirection.Normalize();

        float speed = WalkSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed *= SprintMultiplier;
        }

        // Ground check and jumping
        if (characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Prevent floating above the ground
        }

        if (Input.GetButtonDown("Jump") && characterController.isGrounded)
        {
            velocity.y = Mathf.Sqrt(JumpForce * -2f * Gravity);
        }

        // Apply gravity
        velocity.y += Gravity * Time.deltaTime;

        // Move the character
        characterController.Move((moveDirection * speed + velocity) * Time.deltaTime);
    }

    private void HandleCameraRotation()
    {
        if (PlayerCamera != null)
        {
            float mouseX = Input.GetAxis("Mouse X") * LookSensitivityX;
            float mouseY = Input.GetAxis("Mouse Y") * LookSensitivityY;

            // Rotate the camera for vertical look
            verticalRotation -= mouseY;
            verticalRotation = Mathf.Clamp(verticalRotation, MinYLookAngle, MaxYLookAngle);
            PlayerCamera.localRotation = Quaternion.Euler(verticalRotation, 0, 0);

            // Rotate the player object for horizontal look
            transform.Rotate(Vector3.up * mouseX);
        }
    }
}
