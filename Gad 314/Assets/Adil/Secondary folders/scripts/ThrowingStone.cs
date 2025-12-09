using UnityEngine;

public class ThrowingStone : MonoBehaviour
{
    [Header("Settings")]
    public float noiseRadius = 15f;
    public LayerMask enemyLayer;

    [Header("Audio")]
    public AudioClip impactSound;
    public float volume = 1f;

    private bool _hasHit = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (_hasHit) return;
        if (collision.gameObject.CompareTag("Player")) return;

        _hasHit = true;

        if (impactSound) AudioSource.PlayClipAtPoint(impactSound, transform.position, volume);

        MakeNoise();

        Destroy(gameObject, 3f);
    }

    private void MakeNoise()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, noiseRadius);

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                DinoEnemyAI brain = hit.GetComponent<DinoEnemyAI>();

                if (brain != null)
                {
                    brain.Distract(transform.position);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, noiseRadius);
    }

}
