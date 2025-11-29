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

    void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        transform.Rotate(Vector3.up * mouseX);
    }

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

    void Interact()
    {
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        if (!Physics.Raycast(ray, out RaycastHit hit, interactRange)) return;

        float distance = Vector3.Distance(transform.position, hit.collider.transform.position);

        if (hit.collider.CompareTag("Rock") && heldRock == null && Input.GetKeyDown(KeyCode.E))
            PickupRock(hit.collider.gameObject);

        if (hit.collider.CompareTag("Radar") && !hasRadar && Input.GetKeyDown(KeyCode.E))
            PickupRadar(hit.collider.gameObject);
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

    void ThrowRock()
    {
        if (Input.GetMouseButtonDown(0) && heldRock != null)
        {
            Rigidbody rockRb = heldRock.GetComponent<Rigidbody>();
            Collider rockCol = heldRock.GetComponent<Collider>();
            Physics.IgnoreCollision(GetComponent<Collider>(), rockCol, true);

            heldRock.transform.SetParent(null);

            RockImpact rock = heldRock.GetComponent<RockImpact>();
            Vector3 force = (cameraTransform.forward + Vector3.up * 0.3f) * throwForce;

            if (rock != null)
                rock.Throw(force);
            else
            {
                rockRb.isKinematic = false;
                rockRb.AddForce(force, ForceMode.Impulse);
            }

            heldRock = null;
        }
    }

    void RadarScanInput()
    {
        if (hasRadar && !radarScanned && Input.GetKeyDown(KeyCode.R))
        {
            radarScanned = true;
            if (radarUI != null)
                radarUI.ActivateEnemyDots();
        }
    }

    void UpdateFeedbackUI()
    {
        feedbackUI.HideMessage();

        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        if (!Physics.Raycast(ray, out RaycastHit hit, interactRange)) return;

        if (hit.collider.CompareTag("Rock") && heldRock == null)
            feedbackUI.ShowMessage("Press E to pick up rock");

        else if (hit.collider.CompareTag("Radar") && !hasRadar)
            feedbackUI.ShowMessage("Press E to pick up radar");
    }
}




