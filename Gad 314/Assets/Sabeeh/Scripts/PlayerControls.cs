using System;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float mouseSensitivity = 150f;

    public Transform cameraTransform;

   
    public Transform throwPoint;
    public float throwForce = 15f;

    
    public float radarRange = 30f;

   
    public FeedbackUI feedbackUI;    
    public float interactRange = 5f;

    private bool hasRadar = false;
    private bool radarScanned = false; 
    private Rigidbody rb;
    private GameObject heldRock;
    private float xRotation = 0f;

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
        Interact();
        ThrowRock();
        RadarPing();
        UpdateFeedbackUI(); 
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

   
    void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 move = (transform.right * h + transform.forward * v) * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);
    }

   
    void Interact()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
            {
              
                if (hit.collider.CompareTag("Rock") && heldRock == null)
                    PickupRock(hit.collider.gameObject);

               
                if (hit.collider.CompareTag("Radar") && !hasRadar)
                    PickupRadar(hit.collider.gameObject);
            }
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

       
        RadarUI ui = FindObjectOfType<RadarUI>();
        if (ui != null)
            ui.EnableRadar();

        Destroy(radar);
    }

   
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

    
    void RadarPing()
    {
        if (!hasRadar || radarScanned) return;  

        if (Input.GetKeyDown(KeyCode.R))
        {
            RadarUI ui = FindObjectOfType<RadarUI>();
            if (ui != null)
                ui.Scan();  

            radarScanned = true;  
            feedbackUI.HideMessage(); 
        }
    }

   
    void UpdateFeedbackUI()
    {
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
        {
            if (hit.collider.CompareTag("Rock") && heldRock == null)
            {
                feedbackUI.ShowMessage("Press E to pick up rock");
            }
            else if (hit.collider.CompareTag("Radar") && !hasRadar)
            {
                feedbackUI.ShowMessage("Press E to pick up radar");
            }
            else if (hasRadar && !radarScanned) 
            {
                feedbackUI.ShowMessage("Press R to scan radar");
            }
            else
            {
                feedbackUI.HideMessage();
            }
        }
        else
        {
            if (hasRadar && !radarScanned)
                feedbackUI.ShowMessage("Press R to scan radar");
            else
                feedbackUI.HideMessage();
        }
    }
}
