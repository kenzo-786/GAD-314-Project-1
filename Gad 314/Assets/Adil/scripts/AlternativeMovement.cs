using UnityEngine;

public class AlternativeMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float mouseSensitivity = 2f;
    [SerializeField] float gravity = -9.81f;
    [SerializeField] Transform cameraTransform;

    [Header("Crouch Settings")]
    public float crouchHeight = 1f;
    public float standingHeight = 2f;
    public float crouchSpeed = 2f;
    public KeyCode crouchKey = KeyCode.LeftControl;
    public Vector3 moveDir;

    private CharacterController controller;
    private float rotationY;
    private Vector3 velocity;
    private bool isCrouching = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMovement();
        ApplyGravity();
        HandleCrouch();
    }

    private void HandleMovement()
    {
        
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(horizontal, 0f, vertical).normalized;
        // Vector3 moveDir = transform.TransformDirection(move);

        float targetYRotation = cameraTransform.eulerAngles.y;
        moveDir = Quaternion.Euler(0f, targetYRotation, 0f) * move;


        if (move.magnitude >= 0.1f)
        {
            transform.rotation = Quaternion.Euler(0f, targetYRotation, 0f);
            float speed = isCrouching ? crouchSpeed : moveSpeed;
            controller.Move(moveDir * speed * Time.deltaTime);
        }
        else
        {
            moveDir = Vector3.zero;
        }

        
    }

    private void HandleCrouch()
    {
        if (Input.GetKeyDown(crouchKey))
        {
            isCrouching = true;

         
            controller.height = crouchHeight;
            controller.center = new Vector3(0, crouchHeight / 8f, 0);

           
            cameraTransform.localPosition = new Vector3(
                cameraTransform.localPosition.x,
                crouchHeight - 0.3f,
                cameraTransform.localPosition.z
            );
        }

        if (Input.GetKeyUp(crouchKey))
        {
            isCrouching = false;

            controller.height = standingHeight;
            controller.center = new Vector3(0, standingHeight / 8f, 0);

            cameraTransform.localPosition = new Vector3(
                cameraTransform.localPosition.x,
                standingHeight - 0.3f,
                cameraTransform.localPosition.z
            );
        }
}

    private void ApplyGravity()
    {
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;
        else
            velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}