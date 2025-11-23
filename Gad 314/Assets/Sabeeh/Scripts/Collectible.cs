using UnityEngine;

public class Collectible : MonoBehaviour
{
    public float rotateSpeed = 90f;

    private void Update()
    {
        
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            

            Destroy(gameObject);
        }
    }
}
