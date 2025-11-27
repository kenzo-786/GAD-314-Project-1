using UnityEngine;
using System.Collections;
public class EnemyAI : MonoBehaviour
{
    public float hearRange = 30f;
    public float moveSpeed = 3f;
    public float stoppingDistance = 0.2f;
    public float investigateDuration = 2f;

    private Rigidbody rb;
    private Vector3 targetPosition;
    private bool movingToNoise = false;
    private float investigateTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

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

        Vector3 direction = targetPosition - transform.position;
        direction.y = 0;
        float distance = direction.magnitude;

        if (distance > stoppingDistance)
        {
            // Move only if not too close
            Vector3 move = direction.normalized * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + move);

            // Rotate smoothly
            Quaternion targetRot = Quaternion.LookRotation(direction);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRot, 0.15f));
        }
        else
        {
            // At target, start investigate timer
            investigateTimer += Time.fixedDeltaTime;
            if (investigateTimer >= investigateDuration)
            {
                movingToNoise = false;
                investigateTimer = 0f;
            }
        }
    }

    void OnRockNoise(Vector3 pos)
    {
        if (Vector3.Distance(transform.position, pos) <= hearRange)
        {
            targetPosition = pos;
            movingToNoise = true;
            investigateTimer = 0f;
        }
    }
}