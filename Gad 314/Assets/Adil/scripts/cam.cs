using UnityEngine;

public class cam : MonoBehaviour
{

    public Transform orientation;
    public Transform player;
    public Transform playerobj;
    public Rigidbody rb;

    public float rotationspeed;


    public void start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void update()
    {
        Vector3 view = player.position - new Vector3(transform.position.x, transform.position.y, transform.position.z);
        orientation.forward = view.normalized;

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("vertical");
        Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (inputDir != Vector3.zero)
        {
            playerobj.forward = Vector3.Slerp(playerobj.forward, inputDir.normalized, Time.deltaTime * rotationspeed);

        }
    }


}



