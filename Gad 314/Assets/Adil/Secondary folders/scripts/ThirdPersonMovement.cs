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
    public float gravity = -22f;
    public float gravityMultiplier = 2.5f;

    [Header("Dash")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    [Header("References")]
    public Transform cameraTransform;

    private CharacterController _controller;
    private float _turnSmoothVelocity;
    private Vector3 _velocity;
    private bool _isGrounded;
    private bool _isDashing;
    private float _lastDashTime;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();

        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    private void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.CurrentState != GameState.Gameplay)
            return;

        Vector2 moveInput = InputReader.Instance.GetMoveInput();
        bool jumpDown = InputReader.Instance.GetJumpDown();
        bool dashDown = InputReader.Instance.GetDashDown();
        bool sprintHeld = InputReader.Instance.GetSprintHeld();

        _isGrounded = _controller.isGrounded;
        if (_isGrounded && _velocity.y < 0) _velocity.y = -2f;

        if (dashDown && Time.time >= _lastDashTime + dashCooldown)
        {
            StartCoroutine(DashRoutine(moveInput));
        }

        if (_isDashing) return;

        HandleMovement(moveInput, sprintHeld);
        HandleJump(jumpDown);
        ApplyGravity();
    }

    private void HandleMovement(Vector2 input, bool isSprinting)
    {
        if (input.sqrMagnitude < 0.01f) return;
        Vector3 direction = new Vector3(input.x, 0f, input.y).normalized;
        float targetSpeed = isSprinting ? sprintSpeed : walkSpeed;

        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);

        Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        _controller.Move(moveDir.normalized * targetSpeed * Time.deltaTime);

    }

    private void HandleJump(bool isJumping)
    {
        if (isJumping && _isGrounded)
        {
            _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    private IEnumerator DashRoutine(Vector2 inputDir)
    {
        _isDashing = true;
        _lastDashTime = Time.time;

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

    private void ApplyGravity()
    {
        if (_velocity.y < 0) _velocity.y += gravity * gravityMultiplier * Time.deltaTime;
        else _velocity.y += gravity * Time.deltaTime;

        _controller.Move(_velocity * Time.deltaTime);
    }
}
