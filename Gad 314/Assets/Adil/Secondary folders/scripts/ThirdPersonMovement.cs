using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonMovement : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 6f;
    public float sprintSpeed = 9f;
    public float turnSmoothTime = 0.1f;

    [Header("Jumping & Gravity")]
    public float jumpHeight = 1.2f;
    public float gravity = -15f;
    public float gravityMultiplier = 2.5f;

    [Header("Dash")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    [Header("Ground Detection")]
    public LayerMask groundLayers;
    public float groundCheckRadius = 0.25f;
    public float groundCheckOffset = 0.05f;

    [Header("References")]
    public Transform cameraTransform;
    public Animator animator;

    private CharacterController _controller;
    private float _turnSmoothVelocity;
    private Vector3 _verticalVelocity;
    private bool _isGrounded;
    private bool _isDashing;
    private float _lastDashTime;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();

        if (animator == null) animator = GetComponentInChildren<Animator>();

        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    private void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.CurrentState != GameState.Gameplay)
        {
            if (animator) animator.SetFloat("Speed", 0f);
            return;
        }

        Vector2 moveInput = InputReader.Instance.GetMoveInput();
        bool jumpDown = InputReader.Instance.GetJumpDown();
        bool dashDown = InputReader.Instance.GetDashDown();
        bool sprintHeld = InputReader.Instance.GetSprintHeld();

        _isGrounded = CheckGround();

        if (_isGrounded && _verticalVelocity.y < 0)
        {
            _verticalVelocity.y = -2f;
        }

        if (dashDown && Time.time >= _lastDashTime + dashCooldown)
        {
            StartCoroutine(DashRoutine(moveInput));
        }

        if (_isDashing) return;

        Vector3 horizontalMove = CalculateHorizontalMovement(moveInput, sprintHeld);

        if (jumpDown && _isGrounded)
        {
            _verticalVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            if (animator) animator.SetTrigger("Jump");
        }

        if (_verticalVelocity.y < 0)
        {
            _verticalVelocity.y += gravity * gravityMultiplier * Time.deltaTime;
        }
        else
        {
            _verticalVelocity.y += gravity * Time.deltaTime;
        }

        Vector3 finalMovement = (horizontalMove * Time.deltaTime) + (_verticalVelocity * Time.deltaTime);
        _controller.Move(finalMovement);

        UpdateAnimations(moveInput, sprintHeld);
    }

    private bool CheckGround()
    {
        Vector3 spherePosition = transform.position + Vector3.down * groundCheckOffset;
        return Physics.CheckSphere(spherePosition, groundCheckRadius, groundLayers, QueryTriggerInteraction.Ignore);
    }

    private Vector3 CalculateHorizontalMovement(Vector2 input, bool isSprinting)
    {
        if (input.sqrMagnitude < 0.01f) return Vector3.zero;

        Vector3 direction = new Vector3(input.x, 0f, input.y).normalized;
        float targetSpeed = isSprinting ? sprintSpeed : walkSpeed;

        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);

        Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

        return moveDir.normalized * targetSpeed;
    }

    private void UpdateAnimations(Vector2 input, bool isSprinting)
    {
        if (animator == null) return;

        float targetValue = 0f;

        if (input.sqrMagnitude > 0.01f)
        {
            targetValue = isSprinting ? 1.0f : 0.5f;
        }

        animator.SetFloat("Speed", targetValue, 0.1f, Time.deltaTime);
        animator.SetBool("IsGrounded", _isGrounded);
    }

    private IEnumerator DashRoutine(Vector2 inputDir)
    {
        _isDashing = true;
        _lastDashTime = Time.time;

        if (animator) animator.SetTrigger("Dash");

        Vector3 dashDirection = transform.forward;

        if (inputDir.sqrMagnitude > 0.01f)
        {
            float targetAngle = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            dashDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            transform.rotation = Quaternion.LookRotation(dashDirection);
        }

        float startTime = Time.time;
        while (Time.time < startTime + dashDuration)
        {
            _controller.Move(dashDirection.normalized * dashSpeed * Time.deltaTime);
            yield return null;
        }
        _isDashing = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.5f);
        Vector3 spherePosition = transform.position + Vector3.down * groundCheckOffset;
        Gizmos.DrawSphere(spherePosition, groundCheckRadius);
    }
}
