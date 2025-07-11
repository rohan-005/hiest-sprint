using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 6f;
    public float sprintMultiplier = 1.5f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;

    [Header("Dash Settings")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 5f;
    private bool isDashing = false;
    private float dashTimer = 0f;
    private float lastDashTime = -Mathf.Infinity;
    private Vector3 dashDirection;

    [Header("Mouse Look Settings")]
    public float mouseSensitivity = 350f;
    public Transform playerBody;

    [Header("Cameras")]
    public Transform fpsCameraTransform;
    public Transform topCameraTransform;
    private Camera fpsCam;
    private Camera topCam;
    private bool usingFPS = true;
    [HideInInspector] public bool isDead = false;

    [Header("Animator")]
    public Animator animator;

    private CharacterController controller;
    private float verticalVelocity;
    private bool isGrounded;
    private float xRotation = 14.0f;
    private Vector3 recoilDirection = Vector3.zero;
private float recoilDecay = 10f;


    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;

        if (playerBody == null)
            playerBody = transform;

        fpsCam = fpsCameraTransform.GetComponent<Camera>();
        topCam = topCameraTransform.GetComponent<Camera>();

        SwitchToFPS();
    }

    void Update()
    {
        if (isDead) return;
        HandleCameraSwitch();
        HandleMouseLook();
        HandleMovement();
    }

    void HandleCameraSwitch()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            usingFPS = !usingFPS;
            if (usingFPS) SwitchToFPS();
            else SwitchToTop();
        }
    }

    void SwitchToFPS()
    {
        fpsCam.enabled = true;
        topCam.enabled = false;

        fpsCam.GetComponent<AudioListener>().enabled = true;
        topCam.GetComponent<AudioListener>().enabled = false;

    }

    void SwitchToTop()
    {
        fpsCam.enabled = false;
        topCam.enabled = true;

        fpsCam.GetComponent<AudioListener>().enabled = false;
        topCam.GetComponent<AudioListener>().enabled = true;

    }


    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;

        if (usingFPS)
        {
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -15f, 15f);

            fpsCameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }

        playerBody.Rotate(Vector3.up * mouseX);
    }

    void HandleMovement()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && verticalVelocity < 0)
            verticalVelocity = -2f;

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 moveInput = transform.right * moveX + transform.forward * moveZ;

        // Dash logic
        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time >= lastDashTime + dashCooldown && moveInput.magnitude > 0.1f)
        {
            isDashing = true;
            dashTimer = dashDuration;
            lastDashTime = Time.time;
            dashDirection = moveInput.normalized;
        }

        Vector3 finalMove = Vector3.zero;

        if (isDashing)
        {
            finalMove = dashDirection * dashSpeed;
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0f)
                isDashing = false;
        }
        else
        {
            float currentSpeed = moveSpeed;
            bool isSprinting = Input.GetKey(KeyCode.LeftControl);
            if (isSprinting)
                currentSpeed *= sprintMultiplier;

            finalMove = moveInput * currentSpeed;

            if (animator != null)
            {
                animator.SetBool("Grounded", isGrounded);
                animator.SetBool("run", isSprinting && moveInput.magnitude > 0.1f);
                animator.SetBool("walk", !isSprinting && moveInput.magnitude > 0.1f);
            }
        }

        controller.Move(finalMove * Time.deltaTime);

        // Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Apply gravity
        verticalVelocity += gravity * Time.deltaTime;
        controller.Move(Vector3.up * verticalVelocity * Time.deltaTime);

        // Apply recoil effect
        if (recoilDirection.magnitude > 0.1f)
        {
            finalMove += recoilDirection;
            recoilDirection = Vector3.Lerp(recoilDirection, Vector3.zero, recoilDecay * Time.deltaTime);
        }


    }
    public void ApplyRecoil(Vector3 direction, float force)
{
    recoilDirection = direction.normalized * force;
}

}
