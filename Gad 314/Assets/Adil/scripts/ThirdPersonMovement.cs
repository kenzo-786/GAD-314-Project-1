using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonMovement : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 6f;
    public float sprintSpeed = 12f;

    public float turnSmoothTime = 0.1f;

    [Header("Jumping & Gravity")]
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;
    public float gravityMultiplier = 2f;

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
    private float _dashTimeLeft;
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
        _isGrounded = _controller.isGrounded;

        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;

            HandleDash();
        }
        if (_isDashing) return;

        HandleMovement();
        HandleJump();
        ApplyGravity();
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        float targetSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;

            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            _controller.Move(moveDir.normalized * targetSpeed * Time.deltaTime);
        }
    }

    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    private void HandleDash()
    {
        if (Input.GetKeyDown(KeyCode.E) && Time.time >= _lastDashTime + dashCooldown)
        {
            StartCoroutine(DashRoutine());
        }
    }

    private IEnumerator DashRoutine()
    {
        _isDashing = true;
        _lastDashTime = Time.time;

        Vector3 dashDir;

        if (_controller.velocity.sqrMagnitude > 0.1f)
        {
            dashDir = transform.forward;
        }
        else
        {
            Vector3 camForward = cameraTransform.forward;
            camForward.y = 0;
            dashDir = camForward.normalized;

            if (dashDir.sqrMagnitude > 0.01f)
            {
                transform.rotation = Quaternion.LookRotation(dashDir);
            }
        }

        float startTime = Time.time;

        while (Time.time < startTime + dashDuration)
        {
            _controller.Move(dashDir * dashSpeed * Time.deltaTime);
            yield return null;
        }

        _isDashing = false;
    }

    private void ApplyGravity()
    {
        if (_velocity.y < 0)
        {
            _velocity.y += gravity * gravityMultiplier * Time.deltaTime;
        }
        else
        {
            _velocity.y += gravity * Time.deltaTime;
        }

        _controller.Move(_velocity * Time.deltaTime);
    }
}
