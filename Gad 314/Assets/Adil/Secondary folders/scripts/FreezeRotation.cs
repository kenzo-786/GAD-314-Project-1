using UnityEngine;

public class FreezeRotation : MonoBehaviour
{
    [Header("Settings")]
    public bool freezeX = true;
    public bool freezeY = true;
    public bool freezeZ = true;

    public bool useSpecificRotation = false;
    public Vector3 targetRotation = Vector3.zero;

    private Quaternion _fixedRotation;

    private void Start()
    {
        if (useSpecificRotation)
        {
            _fixedRotation = Quaternion.Euler(targetRotation);
        }
        else
        {
            _fixedRotation = transform.rotation;
        }
    }

    private void LateUpdate()
    {
        Vector3 currentEuler = transform.rotation.eulerAngles;
        Vector3 fixedEuler = _fixedRotation.eulerAngles;

        float x = freezeX ? fixedEuler.x : currentEuler.x;
        float y = freezeY ? fixedEuler.y : currentEuler.y;
        float z = freezeZ ? fixedEuler.z : currentEuler.z;

        transform.rotation = Quaternion.Euler(x, y, z);
    }
}

