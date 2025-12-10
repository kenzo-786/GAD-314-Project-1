using UnityEngine;
using TMPro;

[RequireComponent(typeof(CharacterController))]
public class PetController : MonoBehaviour
{
    [Header("Status")]
    public bool isBroken = false;

    [Header("Hover Settings")]
    public float hoverHeight = 2.0f;
    public float hoverForce = 5.0f;
    public LayerMask groundLayer;

    [Header("Movement Settings")]
    public float moveSpeed = 4.0f;
    public float rotationSpeed = 5.0f;
    public float acceleration = 2.5f;
    public float maxAltitude = 5.0f;
    public float maxControlRange = 50.0f;

    public bool invertControls = true;

    [Header("Follow AI")]
    public Transform playerTarget;
    public float followDistance = 3.0f;
    public float teleportDistance = 20.0f;

    [Header("References")]
    public GameObject signalWarningUI;

    private CharacterController _cc;
    private Vector3 _velocity;
    private bool _isControlled;

    private void Start()
    {
        _cc = GetComponent<CharacterController>();

        if (groundLayer == 0) groundLayer = 1;

        if (playerTarget == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p) playerTarget = p.transform;
        }

        if (playerTarget != null)
        {
            Collider playerCollider = playerTarget.GetComponent<Collider>();
            if (playerCollider != null)
            {
                Physics.IgnoreCollision(_cc, playerCollider);

                Collider[] petColliders = GetComponentsInChildren<Collider>();
                foreach (var c in petColliders)
                {
                    if (!c.isTrigger)
                    {
                        Physics.IgnoreCollision(c, playerCollider);
                    }
                }
            }
        }

        if (playerTarget != null && !isBroken)
        {
            SafeTeleport(playerTarget.position + Vector3.up);
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnStateChanged += OnStateChanged;
            OnStateChanged(GameManager.Instance.CurrentState);
        }
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnStateChanged -= OnStateChanged;
    }

    private void OnStateChanged(GameState newState)
    {
        _isControlled = (newState == GameState.PetControl);
        if (signalWarningUI) signalWarningUI.SetActive(false);

        if (!_isControlled)
        {
            _velocity = Vector3.zero;
        }
    }

    public void Repair()
    {
        isBroken = false;
    }

    private void Update()
    {
        if (isBroken) return;

        if (_isControlled)
        {
            HandleManualMovement();
        }
        else
        {
            HandleAIFollow();
        }

        ApplyHoverAndGravity();

        _cc.Move(_velocity * Time.deltaTime);
    }

    private void HandleManualMovement()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if (invertControls)
        {
            h = -h;
            v = -v;
        }

        bool jump = Input.GetButton("Jump");
        bool descend = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.C);

        Transform activeCamTransform = null;

        if (Camera.main != null && Camera.main.isActiveAndEnabled)
        {
            activeCamTransform = Camera.main.transform;
        }
        else
        {
            foreach (var cam in Camera.allCameras)
            {
                if (cam.isActiveAndEnabled)
                {
                    activeCamTransform = cam.transform;
                    break;
                }
            }
        }

        Vector3 camFwd = Vector3.forward;
        Vector3 camRight = Vector3.right;

        if (activeCamTransform != null)
        {
            camFwd = activeCamTransform.forward;
            camRight = activeCamTransform.right;
            camFwd.y = 0;
            camRight.y = 0;
            camFwd.Normalize();
            camRight.Normalize();
        }

        Vector3 targetDir = (camFwd * v + camRight * h).normalized;

        Vector3 targetVelocity = Vector3.zero;

        if (targetDir.magnitude > 0.1f)
        {
            bool isBlocked = false;
            if (playerTarget != null)
            {
                float dist = Vector3.Distance(transform.position, playerTarget.position);
                if (dist >= maxControlRange)
                {
                    Vector3 dirToPlayer = (playerTarget.position - transform.position).normalized;
                    if (Vector3.Dot(targetDir, dirToPlayer) < 0)
                    {
                        isBlocked = true;
                        if (signalWarningUI) signalWarningUI.SetActive(true);
                    }
                    else
                    {
                        if (signalWarningUI) signalWarningUI.SetActive(false);
                    }
                }
                else
                {
                    if (signalWarningUI) signalWarningUI.SetActive(false);
                }
            }

            if (!isBlocked)
            {
                targetVelocity = targetDir * moveSpeed;
            }
        }
        else
        {
            if (signalWarningUI) signalWarningUI.SetActive(false);
        }

        _velocity.x = Mathf.Lerp(_velocity.x, targetVelocity.x, Time.deltaTime * acceleration);
        _velocity.z = Mathf.Lerp(_velocity.z, targetVelocity.z, Time.deltaTime * acceleration);

        if (jump)
        {
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 100f, groundLayer, QueryTriggerInteraction.Ignore))
            {
                if (hit.distance < maxAltitude)
                {
                    _velocity.y = Mathf.Lerp(_velocity.y, moveSpeed / 2f, Time.deltaTime * 2f);
                }
                else
                {
                    if (_velocity.y > 0) _velocity.y = 0;
                }
            }
            else
            {
                if (_velocity.y > 0) _velocity.y = 0;
            }
        }
        else if (descend)
        {
            _velocity.y = Mathf.Lerp(_velocity.y, -moveSpeed / 2f, Time.deltaTime * 2f);
        }
    }

    private void HandleAIFollow()
    {
        if (!playerTarget) return;

        float dist = Vector3.Distance(transform.position, playerTarget.position);

        if (dist > teleportDistance)
        {
            SafeTeleport(playerTarget.position + Vector3.up);
            return;
        }

        if (dist > followDistance)
        {
            Vector3 dir = (playerTarget.position - transform.position).normalized;
            dir.y = 0;

            if (dir != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 5f);
                _velocity.x = dir.x * moveSpeed * 0.8f;
                _velocity.z = dir.z * moveSpeed * 0.8f;
            }
        }
        else
        {
            _velocity.x = 0;
            _velocity.z = 0;
        }
    }

    private void SafeTeleport(Vector3 targetPos)
    {
        if (_cc != null)
        {
            _cc.enabled = false;
            transform.position = targetPos;
            _cc.enabled = true;
        }
        else
        {
            transform.position = targetPos;
        }

        _velocity = Vector3.zero;
    }

    private void ApplyHoverAndGravity()
    {
        bool manualVertical = _isControlled && (Input.GetButton("Jump") || Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.C));

        if (manualVertical) return;

        Ray ray = new Ray(transform.position + Vector3.up, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, 50f, groundLayer, QueryTriggerInteraction.Ignore))
        {
            float groundY = hit.point.y;

            float bobOffset = _isControlled ? 0f : (Mathf.Sin(Time.time * 2f) * 0.1f);
            float targetY = groundY + hoverHeight + bobOffset;

            float diff = targetY - transform.position.y;

            _velocity.y = Mathf.Lerp(_velocity.y, diff * hoverForce, Time.deltaTime * 2f);
        }
        else
        {
            _velocity.y += -9.81f * Time.deltaTime;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position + Vector3.up, transform.position + Vector3.down * hoverHeight);
    }
}
