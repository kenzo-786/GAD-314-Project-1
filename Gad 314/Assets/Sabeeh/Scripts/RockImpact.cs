using System;
using UnityEngine;

public class RockImpact : MonoBehaviour
{
    private bool thrown = false;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Throw(Vector3 force)
    {
        thrown = true;
        rb.isKinematic = false;
        rb.velocity = Vector3.zero;
        rb.AddForce(force, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (thrown)
        {
            thrown = false;

            // Notify enemies that the rock landed
            RockDistraction.RockThrown(transform.position);

            // Optional: you can add visual effects here
            // e.g., Instantiate(impactEffect, transform.position, Quaternion.identity);
        }
    }
}
