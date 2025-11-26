using UnityEngine;

public class RockImpact : MonoBehaviour
{
    private bool hasHit = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (hasHit) return;

        // Fix #2: ignore collision with player
        if (collision.collider.CompareTag("Player")) return;

        hasHit = true;

        RockDistraction.Trigger(transform.position);

        // Optional: debug to confirm impact
        Debug.Log("Rock impact at: " + transform.position);
    }
}
