using UnityEngine;
using TMPro;

[RequireComponent(typeof(CharacterController))]
public class PetController : MonoBehaviour
{
    [Header("Status")]
    public bool isBroken = false;

    [Header("Follow Settings")]
    public Transform playerTarget;
    public float stoppingDistance = 3f;
    public float followSpeed = 5f;
    public float teleportDistance = 15f;

    [Header("Control Settings")]
    public float controlSpeed = 4f;
    public float turnSpeed = 10f;
    public float maxSignalRange = 50f;

    public bool isFirstPerson = false;

    [Header("Physics")]
    public float gravity = -20f;
    public float jumpHeight = 2.5f;
    public bool canDoubleJump = true;

    [Header("UI References")]
    public GameObject signalWarningUI;
    public float warningThreshold = 0.8f;

    [Header("References")]
    public Transform petCameraPos;

    private CharacterController _controller;
    private bool _isControlled = false;
    private Vector3 _velocity;
    private int _jumpCount = 0;
    private float _blinkTimer;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();

        if (petCameraPos != null && petCameraPos.parent == transform)
        {
            petCameraPos.localScale = Vector3.one;
        }

        if (playerTarget == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player) playerTarget = player.transform;
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnStateChanged += HandleStateChange;
        }
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnStateChanged -= HandleStateChange;
    }

    public void Repair()
    {
        isBroken = false;
        Debug.Log("Pet Rebooted!");
    }

    private void HandleStateChange(GameState newState)
    {
        _isControlled = (newState == GameState.PetControl);
        _velocity = Vector3.zero;
        if (signalWarningUI) signalWarningUI.SetActive(false);
    }

    private void Update()
    {
        ApplyGravity();

        if (isBroken) return;

        if (_isControlled)
        {
            HandleManualMovement();
        }
        else
        {
            HandleSimpleFollow();
        }
    }

    private void UpdateSignalUI()
    {
        if (playerTarget == null || signalWarningUI == null) return;

        float dist = Vector3.Distance(transform.position, playerTarget.position);
        float limit = maxSignalRange;

        if (dist > limit * warningThreshold)
        {
            _blinkTimer += Time.deltaTime * 5f;
            signalWarningUI.SetActive(Mathf.Sin(_blinkTimer) > 0);
        }
        else
        {
            signalWarningUI.SetActive(false);
        }
    }

    private void HandleSimpleFollow()
    {
        if (playerTarget == null) return;

        float dist = Vector3.Distance(transform.position, playerTarget.position);

        if (dist > teleportDistance)
        {
            transform.position = playerTarget.position - (playerTarget.forward * 1.5f) + (Vector3.up * 0.5f);
            return;
        }

        if (dist > stoppingDistance)
        {
            Vector3 direction = (playerTarget.position - transform.position).normalized;
            direction.y = 0;

            if (direction != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), turnSpeed * Time.deltaTime);

            _controller.Move(transform.forward * followSpeed * Time.deltaTime);
        }
    }

    private void HandleManualMovement()
    {
        Vector2 input = InputReader.Instance.GetMoveInput();
        bool jumpDown = InputReader.Instance.GetJumpDown();

        Vector3 moveDir = Vector3.zero;

        if (isFirstPerson)
        {
            moveDir = transform.right * input.x + transform.forward * input.y;
        }
        else
        {
            if (input.sqrMagnitude > 0.01f)
            {
                if (Camera.main != null)
                {
                    Vector3 camForward = Camera.main.transform.forward;
                    Vector3 camRight = Camera.main.transform.right;
                    camForward.y = 0; camRight.y = 0;
                    camForward.Normalize(); camRight.Normalize();
                    moveDir = (camForward * input.y + camRight * input.x).normalized;
                }

                if (moveDir != Vector3.zero)
                {
                    Quaternion targetRot = Quaternion.LookRotation(moveDir);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, turnSpeed * Time.deltaTime);
                }
            } 
        }
        if (playerTarget != null)
        {
            float distToPlayer = Vector3.Distance(transform.position, playerTarget.position);

            if (distToPlayer >= maxSignalRange)
            {
                Vector3 dirToPlayer = (playerTarget.position - transform.position).normalized;

                if (Vector3.Dot(moveDir, dirToPlayer) < 0)
                {
                    moveDir = Vector3.zero;
                }
            }
        }

            if (moveDir.magnitude > 0.001f)
        {
            _controller.Move(moveDir * controlSpeed * Time.deltaTime);
        }
            
        if (jumpDown)
        {
            if (_controller.isGrounded)
            {
                _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                _jumpCount = 1;
            }
            else if (canDoubleJump && _jumpCount < 2)
            {
                _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                _jumpCount++;
            }
        }

        if (Vector3.Distance(transform.position, playerTarget.position) > maxSignalRange)
        {
            Debug.LogWarning("Signal Lost!");
           // GameManager.Instance.SetState(GameState.Gameplay);
        }
    }

    private void ApplyGravity()
    {
        if (_controller.isGrounded)
        {
            _jumpCount = 0;
            if (_velocity.y < 0) _velocity.y = -2f;
        }

        _velocity.y += gravity * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);
    }
}
