using UnityEngine;

public class ThrowingStone : MonoBehaviour
{
    [Header("Settings")]
    public float noiseRadius = 20f;
    public LayerMask enemyLayer;

    [Header("Audio")]
    public AudioClip impactSound;
    public float volume = 1f;

    private bool _hasHit = false;
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();

        _rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

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
            DinoNavMeshAI brain = hit.GetComponent<DinoNavMeshAI>();

            if (brain != null)
            {
                brain.Distract(transform.position);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, noiseRadius);
    }

}
