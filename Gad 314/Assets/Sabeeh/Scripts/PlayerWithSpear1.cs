using UnityEngine;

public class PlayerWithSpear1 : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float jumpForce = 8f;
    public float gravity = -20f;

    CharacterController controller;
    Vector3 velocity;

  
    
    public float mouseSensitivity = 200f;
    public Transform cam;
    float xRotation = 0f;

  
    public GameObject spearPrefab;
    public Transform throwPoint;
    public float throwForce = 25f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Look();
        Move();
        ThrowSpear();
    }

    
    void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        cam.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.Rotate(Vector3.up * mouseX);
    }

   
    void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * moveSpeed * Time.deltaTime);

        // Gravity
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && controller.isGrounded)
            velocity.y = jumpForce;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    
    void ThrowSpear()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject spear = Instantiate(spearPrefab, throwPoint.position, throwPoint.rotation);

            Rigidbody rb = spear.GetComponent<Rigidbody>();
            rb.useGravity = true;

            rb.AddForce(throwPoint.forward * throwForce, ForceMode.Impulse);

            // Add spear stick script automatically
            spear.AddComponent<SpearStick>();
        }
    }
}
public class SpearStick : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        rb.isKinematic = true;      // Freeze spear
        rb.velocity = Vector3.zero;

        transform.SetParent(collision.transform); // Stick into object
    }
}

