using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    public Rigidbody rb;
    public Transform cameraTransform;
    public GameObject miniMapCanvas;

    [Header("Movement Settings")]
    public float walkSpeed = 4f;
    public float sprintSpeed = 6f;
    public float crouchSpeed = 2.5f;
    public float jumpForce = 5f;

    [Header("Crouch Settings")]
    public float crouchHeight = 0.5f;
    public float standingHeight = 1f;

    [Header("Camera Settings")]
    public Vector3 cameraOffset = new Vector3(0f, 5f, -5f);
    public float cameraSmoothSpeed = 5f;

    private Vector3 moveInput;
    private bool isGrounded;
    private bool isCrouching;
    private bool isSprinting;

    void Start()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody>();

        rb.freezeRotation = true;

        if (miniMapCanvas != null)
            miniMapCanvas.SetActive(true); 
    }

    void Update()
    {
        HandleInput();
        HandleCrouchVisual();
        HandleCameraFollow();
    }

    void FixedUpdate()
    {
        HandleMovement();
        HandleJump();
    }

    void HandleInput()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        moveInput = transform.right * x + transform.forward * z;

        isCrouching = Input.GetKey(KeyCode.C);
        isSprinting = Input.GetKey(KeyCode.LeftShift) && !isCrouching;
    }

    void HandleMovement()
    {
        float speed = walkSpeed;
        if (isCrouching) speed = crouchSpeed;
        else if (isSprinting) speed = sprintSpeed;

        Vector3 velocity = moveInput.normalized * speed;
        rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);
    }

    void HandleJump()
    {
        isGrounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, 1.2f);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
        }
    }

    void HandleCrouchVisual()
    {
        float targetHeight = isCrouching ? crouchHeight : standingHeight;
        Vector3 scale = transform.localScale;
        scale.y = Mathf.Lerp(scale.y, targetHeight, Time.deltaTime * 10f);
        transform.localScale = scale;
    }

    void HandleCameraFollow()
    {
        if (cameraTransform == null) return;

        Vector3 targetPos = transform.position + cameraOffset;
        cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetPos, cameraSmoothSpeed * Time.deltaTime);
        cameraTransform.LookAt(transform.position + Vector3.up * 1f);
    }
}
