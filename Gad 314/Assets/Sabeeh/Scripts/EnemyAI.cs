using UnityEngine;
using System.Collections;
public class EnemyAI : MonoBehaviour
{
    public float hearRange = 30f;
    public float moveSpeed = 3f;
    public float groundCheckDistance = 1.3f;

    private Renderer rend;
    private Rigidbody rb;

    private Vector3 targetPosition;
    private bool movingToNoise = false;

    void Start()
    {
        rend = GetComponent<Renderer>();
        rb = GetComponent<Rigidbody>();

        // Lock physics rotation (prevents tipping over)
        rb.constraints = RigidbodyConstraints.FreezeRotationX |
                         RigidbodyConstraints.FreezeRotationZ;

        RockDistraction.onRockThrown += OnRockNoise;
    }

    void OnDestroy()
    {
        RockDistraction.onRockThrown -= OnRockNoise;
    }

    void FixedUpdate()
    {
        if (!movingToNoise) return;

        Vector3 direction = (targetPosition - transform.position);
        direction.y = 0; // Keep movement horizontal

        Vector3 move = direction.normalized * moveSpeed * Time.fixedDeltaTime;

        rb.MovePosition(rb.position + move);

        // Face movement direction
        if (direction != Vector3.zero)
        {
            Quaternion rot = Quaternion.LookRotation(direction);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, rot, 0.15f));
        }

        // Stop if close
        if (Vector3.Distance(transform.position, targetPosition) < 0.8f)
            movingToNoise = false;
    }

    void OnRockNoise(Vector3 pos)
    {
        if (Vector3.Distance(transform.position, pos) <= hearRange)
        {
            targetPosition = pos;
            movingToNoise = true;
        }
    }

    // Radar highlight
    public void Highlight()
    {
        StartCoroutine(FlashRed());
    }

    IEnumerator FlashRed()
    {
        Color original = rend.material.color;
        rend.material.color = Color.red;
        yield return new WaitForSeconds(0.3f);
        rend.material.color = original;
    }
}