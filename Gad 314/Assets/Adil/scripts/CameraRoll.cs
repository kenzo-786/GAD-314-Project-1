using Unity.Mathematics;
using UnityEngine;

public class CameraRoll : MonoBehaviour
{
    public Transform Target;
    public float rotationSpeed = .02f;
    public float positionSpeed = .01f;


    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, Target.position, positionSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, Target.rotation, rotationSpeed);
    }
}
