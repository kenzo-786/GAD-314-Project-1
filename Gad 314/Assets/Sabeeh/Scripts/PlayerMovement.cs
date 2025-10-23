using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    [HideInInspector] public bool isActive = false;

    void Update()
    {
        if (!isActive) return;

        float h = Input.GetAxis("Horizontal");  // A/D or Left/Right
        float v = Input.GetAxis("Vertical");    // W/S or Up/Down

        Vector3 direction = new Vector3(h, 0, v);
        transform.Translate(direction * speed * Time.deltaTime);
    }
}
