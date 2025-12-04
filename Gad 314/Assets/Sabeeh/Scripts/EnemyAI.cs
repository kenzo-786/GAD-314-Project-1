using UnityEngine;
using System.Collections;
public class EnemyAI : MonoBehaviour
{
    public float hearRange = 30f;
    public float moveSpeed = 3f;
    public float stoppingDistance = 0.2f; // base stop distance
    public float stopOffset = 0.02f; // stop 2 cm before rock
    public float investigateDuration = 2f;

    private Rigidbody rb;
    private Vector3 targetPosition;
    private bool hasTarget = false;
    private float investigateTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        RockDistraction.onRockThrown += OnRockNoise;
    }

    void OnDestroy()
    {
        RockDistraction.onRockThrown -= OnRockNoise;
    }

    void FixedUpdate()
    {
        if (!hasTarget) return;

        Vector3 flatEnemy = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 flatTarget = new Vector3(targetPosition.x, 0, targetPosition.z);
        Vector3 direction = flatTarget - flatEnemy;
        float distance = direction.magnitude;

        // STOP before hitting rock
        float finalStopDistance = stoppingDistance + stopOffset;

        if (distance > finalStopDistance)
        {
            // Move with velocity, not MovePosition
            Vector3 move = direction.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(move.x, rb.linearVelocity.y, move.z);

            // Rotate toward rock
            if (direction != Vector3.zero)
            {
                Quaternion targetRot = Quaternion.LookRotation(direction);
                rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRot, 0.2f));
            }
        }
        else
        {
            // Stop movement
            rb.linearVelocity = Vector3.zero;

            investigateTimer += Time.fixedDeltaTime;
            if (investigateTimer >= investigateDuration)
            {
                hasTarget = false;
                investigateTimer = 0f;
            }
        }
    }

    void OnRockNoise(Vector3 pos)
    {
        if (Vector3.Distance(transform.position, pos) <= hearRange)
        {
            targetPosition = pos;
            hasTarget = true;
            investigateTimer = 0;
            Debug.Log(gameObject.name + " heading to rock at " + pos);
        }
    }
}





