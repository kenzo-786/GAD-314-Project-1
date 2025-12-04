using System.Runtime.InteropServices;
using UnityEngine;

public class PetFPCamera : MonoBehaviour
{
    [Header("Settings")]
    public float mouseSensitivity = 150f;
    public Transform petBody;

    private float _xRotation = 0f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (GameManager.Instance != null && !GameManager.Instance.CanMove()) return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -80f, 80f);
        transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);

        if (petBody != null)
        {
            petBody.Rotate(Vector3.up * mouseX);
        }
    }
}
