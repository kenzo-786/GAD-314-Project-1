using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform _camTransform;

    void Start()
    {
        if (Camera.main) _camTransform = Camera.main.transform;
    }

    void LateUpdate()
    {
        if (_camTransform)
        {
            transform.LookAt(transform.position + _camTransform.forward);
        }
    }
}
