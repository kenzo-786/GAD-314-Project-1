using UnityEngine;

public class RockCollisionHelper : MonoBehaviour
{
    private bool triggered = false;
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision col)
    {
        if (triggered) return;

        triggered = true;

      
        //RockDistraction.Trigger(transform.position);

        
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

       
        rb.isKinematic = true;
    }
}

