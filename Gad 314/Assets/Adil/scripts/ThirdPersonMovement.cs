using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonMovement : MonoBehaviour
{
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

    [Header("Rock Interaction")]
    public Transform holdPoint;
    public float throwForce = 15f;
    public float pickupRange = 3f;

    [Header("References")]
    public Transform cameraTransform;

    private CharacterController _controller;
    private float _turnSmoothVelocity;
    private Vector3 _velocity;
    private bool _isGrounded;

    private bool _isDashing;
    private float _lastDashTime;

    private GameObject heldRock;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        _isGrounded = _controller.isGrounded;
        if (_isGrounded && _velocity.y < 0) _velocity.y = -2f;

        HandleDash();

        if (!_isDashing)
        {
            HandleMovement();
            HandleJump();
        }

        ApplyGravity();
        HandleRockInteraction();
    }

    private void HandleMovement()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 dir = new Vector3(h, 0f, v).normalized;

        float speed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;

        if (dir.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            _controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
    }

    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && _isGrounded)
            _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    private void HandleDash()
    {
        if (Input.GetKeyDown(KeyCode.E) && Time.time >= _lastDashTime + dashCooldown)
            StartCoroutine(DashRoutine());
    }

    private IEnumerator DashRoutine()
    {
        _isDashing = true;
        _lastDashTime = Time.time;

        Vector3 dashDir = _controller.velocity.sqrMagnitude > 0.1f ? transform.forward : cameraTransform.forward;
        dashDir.y = 0;
        dashDir.Normalize();
        if (dashDir.sqrMagnitude > 0.01f)
            transform.rotation = Quaternion.LookRotation(dashDir);

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
        _velocity.y += (_velocity.y < 0 ? gravity * gravityMultiplier : gravity) * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);
    }

    private void HandleRockInteraction()
    {
        // Pick up rock (T)
        if (Input.GetKeyDown(KeyCode.T) && heldRock == null)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, pickupRange);
            foreach (Collider hit in hits)
            {
                if (hit.CompareTag("Rock"))
                {
                    heldRock = hit.gameObject;
                    Rigidbody rb = heldRock.GetComponent<Rigidbody>();
                    rb.isKinematic = true;
                    heldRock.transform.position = holdPoint.position;
                    heldRock.transform.rotation = holdPoint.rotation;
                    heldRock.transform.parent = holdPoint;
                    break;
                }
            }
        }

        // Throw rock (Mouse1)
        if (heldRock != null && Input.GetMouseButtonDown(0))
        {
            // Detach
            heldRock.transform.parent = null;

            Rigidbody rb = heldRock.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.velocity = Vector3.zero;

            // Place slightly in front
            heldRock.transform.position = holdPoint.position + cameraTransform.forward * 0.5f;

            // Throw
            Vector3 throwDir = (cameraTransform.forward + Vector3.up * 0.7f).normalized;
            rb.AddForce(throwDir * throwForce, ForceMode.Impulse);

            // Add RockImpact script if not present
            RockImpact rockImpact = heldRock.GetComponent<RockImpact>();
            if (rockImpact == null)
                rockImpact = heldRock.AddComponent<RockImpact>();

            rockImpact.Throw(throwDir * throwForce);

            heldRock = null;
        }

        // Keep rock following hold point
        if (heldRock != null)
        {
            heldRock.transform.position = holdPoint.position;
            heldRock.transform.rotation = holdPoint.rotation;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupRange);
    }
}