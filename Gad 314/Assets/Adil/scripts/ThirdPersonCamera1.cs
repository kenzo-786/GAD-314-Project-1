using UnityEngine;


[RequireComponent(typeof(Camera))]
public class ThirdPersonCamera1 : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target;
    public Vector3 targetOffset = new Vector3(0, 1.5f, 0);

    [Header("Orbit Settings")]
    public float mouseSensitivityX = 150f;
    public float mouseSensitivityY = 120f;
    public bool invertY = false;
    public float rotationSmoothTime = 0.12f;
    public float minVerticalAngle = -30f;
    public float maxVerticalAngle = 60f;

    [Header("Zoom & Collision")]
    public float defaultDistance = 5f;
    public float minDistance = 1f;
    public float maxDistance = 10f;
    public float zoomSpeed = 5f;
    public float collisionLerpSpeed = 10f;
    public float cameraCollisionRadius = 0.2f;
    public LayerMask collisionLayers;

    private float _yaw;
    private float _pitch;
    private float _currentDistance;
    private float _targetDistance;
    private Vector3 _currentVelocity;
    private Vector3 _rotationVelocity;

    private float _inputX;
    private float _inputY;
    private float _inputScroll;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Vector3 angles = transform.eulerAngles;
        _yaw = angles.y;
        _pitch = angles.x;

        _currentDistance = defaultDistance;
        _targetDistance = defaultDistance;

    }

    private void Update()
    {
        HandleInput();
    }

    private void LateUpdate()
    {
        if (target == null) return;

        HandleRotation();
        HandleZoom();
        HandleCollisionAndPosition();

    }

    private void HandleInput()
    {
        _inputX =Input.GetAxis("Mouse X") * mouseSensitivityX * Time.deltaTime;
        _inputY = Input.GetAxis("Mouse Y") * mouseSensitivityY * Time.deltaTime;
        _inputScroll = Input.GetAxis("Mouse ScrollWheel");
    }

    private void HandleRotation()
    {
        _yaw += _inputX;
        _pitch -= invertY ? -_inputY : _inputY;
        _pitch = Mathf.Clamp(_pitch, minVerticalAngle, maxVerticalAngle);

        Vector3 targetRotationEuler = new Vector3(_pitch, _yaw, 0);
    }

    private void HandleZoom()
    {
        if (Mathf.Abs(_inputScroll) > 0.01f)
        {
            _targetDistance -= _inputScroll * zoomSpeed;
            _targetDistance = Mathf.Clamp(_targetDistance, minDistance, maxDistance);
        }
    }

    private void HandleCollisionAndPosition()
    {
        float currentYaw = transform.eulerAngles.y;
        float currentPitch = transform.eulerAngles.x;

        float smoothedYaw = Mathf.SmoothDampAngle(currentYaw, _yaw, ref _rotationVelocity.y, rotationSmoothTime);
        float smoothedPitch = Mathf.SmoothDampAngle(currentPitch, _pitch, ref _rotationVelocity.x, rotationSmoothTime);

        Quaternion currentRotation = Quaternion.Euler(smoothedPitch, smoothedYaw, 0);

        Vector3 targetPos = target.position + targetOffset;
        Vector3 direction = currentRotation * Vector3.back;
        Vector3 desiredPosition = targetPos + (direction * _targetDistance);

        RaycastHit hit;
        float finalDistance = _targetDistance;

        Vector3 castDirection = (desiredPosition - targetPos).normalized;
        float castDistance = Vector3.Distance(targetPos, desiredPosition);

        if (Physics.SphereCast(targetPos, cameraCollisionRadius, castDirection, out hit, castDistance, collisionLayers))
        {
            finalDistance = Mathf.Max(minDistance, hit.distance - cameraCollisionRadius);
        }

        _currentDistance = Mathf.Lerp(_currentDistance, finalDistance, Time.deltaTime * collisionLerpSpeed);

        if (finalDistance < _currentDistance)
        {
            _currentDistance = finalDistance;
        }

        transform.rotation = currentRotation;
        transform.position = targetPos + (direction * _currentDistance);
    }


}
