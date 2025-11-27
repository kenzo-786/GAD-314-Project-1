using System;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float mouseSensitivity = 150f;
    public Transform cameraTransform;

    [Header("Throwing")]
    public Transform throwPoint;
    public float throwForce = 15f;

    [Header("Radar & Feedback")]
    public float interactRange = 5f;
    public FeedbackUI feedbackUI;
    public RadarUI radarUI;
    public float radarRange = 30f;

    private Rigidbody rb;
    private GameObject heldRock;
    private float xRotation = 0f;
    private float hInput, vInput;

    private bool hasRadar = false;
    private bool radarScanned = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.constraints = RigidbodyConstraints.FreezeRotationX |
                         RigidbodyConstraints.FreezeRotationY |
                         RigidbodyConstraints.FreezeRotationZ;

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Look();
        HandleMovementInput();
        Interact();
        ThrowRock();
        UpdateFeedbackUI();
        RadarScanInput();
    }

    void FixedUpdate()
    {
        Move();
    }

    // --- CAMERA ---
    void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        transform.Rotate(Vector3.up * mouseX);
    }

    // --- MOVEMENT ---
    void HandleMovementInput()
    {
        hInput = Input.GetAxis("Horizontal");
        vInput = Input.GetAxis("Vertical");
    }

    void Move()
    {
        Vector3 move = (transform.right * hInput + transform.forward * vInput) * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);
    }

    // --- INTERACT ---
    void Interact()
    {
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        if (!Physics.Raycast(ray, out RaycastHit hit, interactRange)) return;

        float distance = Vector3.Distance(transform.position, hit.collider.transform.position);

        // Rock pickup
        if (hit.collider.CompareTag("Rock") && heldRock == null && distance <= interactRange && Input.GetKeyDown(KeyCode.E))
        {
            PickupRock(hit.collider.gameObject);
        }

        // Radar pickup
        if (hit.collider.CompareTag("Radar") && !hasRadar && distance <= interactRange && Input.GetKeyDown(KeyCode.E))
        {
            PickupRadar(hit.collider.gameObject);
        }
    }

    void PickupRock(GameObject rock)
    {
        heldRock = rock;
        Rigidbody rockRb = heldRock.GetComponent<Rigidbody>();
        rockRb.isKinematic = true;

        heldRock.transform.SetParent(throwPoint);
        heldRock.transform.localPosition = Vector3.forward * 0.3f;
        heldRock.transform.localRotation = Quaternion.identity;
    }

    void PickupRadar(GameObject radar)
    {
        hasRadar = true;

        if (radarUI != null)
            radarUI.EnableRadar();

        Destroy(radar);
    }

    // --- THROW ROCK ---
    void ThrowRock()
    {
        if (Input.GetMouseButtonDown(0) && heldRock != null)
        {
            Rigidbody rockRb = heldRock.GetComponent<Rigidbody>();
            Collider rockCol = heldRock.GetComponent<Collider>();
            Collider playerCol = GetComponent<Collider>();
            Physics.IgnoreCollision(rockCol, playerCol, true);

            if (!heldRock.GetComponent<RockCollisionHelper>())
                heldRock.AddComponent<RockCollisionHelper>();

            heldRock.transform.SetParent(null);
            rockRb.isKinematic = false;
            rockRb.AddForce((cameraTransform.forward + Vector3.up * 0.3f) * throwForce, ForceMode.Impulse);

            heldRock = null;
        }
    }

    // --- RADAR SCAN INPUT ---
    void RadarScanInput()
    {
        if (hasRadar && !radarScanned && Input.GetKeyDown(KeyCode.R))
        {
            radarScanned = true;
            if (radarUI != null)
                radarUI.ActivateEnemyDots(); // show enemies
        }
    }

    // --- FEEDBACK UI ---
    void UpdateFeedbackUI()
    {
        bool showMessage = false;

        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
        {
            float distance = Vector3.Distance(transform.position, hit.collider.transform.position);

            if (hit.collider.CompareTag("Rock") && heldRock == null && distance <= interactRange)
            {
                feedbackUI.ShowMessage("Press E to pick up rock");
                showMessage = true;
            }
            else if (hit.collider.CompareTag("Radar") && !hasRadar && distance <= interactRange)
            {
                feedbackUI.ShowMessage("Press E to pick up radar");
                showMessage = true;
            }
        }

        if (!showMessage)
            feedbackUI.HideMessage();
    }
}

