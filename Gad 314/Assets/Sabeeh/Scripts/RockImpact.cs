using System;
using UnityEngine;

public class RockImpact : MonoBehaviour
{
    private bool thrown = false;
    private Rigidbody rb;
    private bool hasLanded = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearDamping = 3f;
        rb.angularDamping = 5f;
    }

    public void Throw(Vector3 force)
    {
        thrown = true;
        hasLanded = false;

        rb.constraints = RigidbodyConstraints.None;
        rb.isKinematic = false;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.AddForce(force, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!thrown || hasLanded) return;

        // Only stop if we hit the ground
        if (!collision.collider.CompareTag("Ground")) return;

        hasLanded = true;
        thrown = false;

        // Force settle onto ground surface
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hit, 2f))
        {
            transform.position = hit.point;
        }

        // HARD STOP
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Freeze ONLY horizontal movement (Y gravity still applies)
        rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ |
                         RigidbodyConstraints.FreezeRotation;

        // SEND SOUND EVENT
        RockDistraction.RockThrown(transform.position);
    }
}




