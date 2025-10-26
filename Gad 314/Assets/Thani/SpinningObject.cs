using UnityEngine;

public class SpinningObject : MonoBehaviour
{
    void FixedUpdate()
    {
        transform.Rotate(0, 90 * Time.deltaTime, 0);
    }
}
