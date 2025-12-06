using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(CharacterController))]

public class DinoEnemyAI : MonoBehaviour
{
    public enum AIState { Patrol, Chase, Distracted, Attack }

    [Header("Debug")]
    public AIState currentState;

    [Header("Senses")]
    public float sightRange = 15f;
    public float losePlayerRange = 20f;
    public float attackRange = 3f;
    public LayerMask obstacleMask;
    public Transform playerTarget;

    [Header("Territory (New)")]
    public float maxTerritoryRange = 40f;
    private Vector3 _spawnPosition;

    [Header("Patrol Settings")]
    public Transform[] patrolPoints;
    public float patrolSpeed = 3.5f;
    public float chaseSpeed = 6.0f;
    public float waitTime = 2f;

    [Header("Distraction")]
    public float distractionDuration = 5f;

    [Header("Physics")]
    public float gravity = -20f;
    public float turnSpeed = 5f;

    private CharacterController _controller;
    private int _currentPatrolIndex;
    private float _waitTimer;
    private Vector3 _velocity;
    private Vector3 _distractionTarget;
    private float _giveUpTimer;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();

        _controller.stepOffset = 1.0f;
        _controller.slopeLimit = 50f;
        _controller.minMoveDistance = 0f;

        _spawnPosition = transform.position;

        if (playerTarget == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p) playerTarget = p.transform;
        }

        currentState = AIState.Patrol;
    }

    private void Update()
    {
        ApplyGravity();

        if (playerTarget == null) return;

        float distToPlayer = Vector3.Distance(transform.position, playerTarget.position);
        float distToSpawn = Vector3.Distance(transform.position, _spawnPosition);

        if (distToSpawn > maxTerritoryRange && currentState != AIState.Patrol)
        {
            Debug.Log("Too far from nest! Returning home.");
            currentState = AIState.Patrol;
        }

        switch (currentState)
        {
            case AIState.Patrol:
                PatrolLogic(distToPlayer);
                break;
            case AIState.Chase:
                ChaseLogic(distToPlayer);
                break;
            case AIState.Distracted:
                DistractedLogic(distToPlayer);
                break;
            case AIState.Attack:
                AttackLogic(distToPlayer);
                break;
        }
    }

    private bool CanSeePlayer()
    {
        if (playerTarget == null) return false;

        Vector3 eyePos = transform.position + Vector3.up * 1.5f;
        Vector3 playerEyePos = playerTarget.position + Vector3.up * 1.5f;
        Vector3 direction = (playerEyePos - eyePos).normalized;
        float dist = Vector3.Distance(eyePos, playerEyePos);

        Debug.DrawLine(eyePos, playerEyePos, Color.red);

        if (Physics.Raycast(eyePos, direction, out RaycastHit hit, sightRange, obstacleMask))
        {
            if (hit.transform != playerTarget) return false;
        }

        return true;
    }

    private void MoveTowards(Vector3 targetPos, float speed)
    {
        Vector3 direction = (targetPos - transform.position).normalized;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            Quaternion lookRot = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * turnSpeed);
            _controller.Move(direction * speed * Time.deltaTime);
        }
    }

    private void ApplyGravity()
    {

        if (_controller.isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }

        _velocity.y += gravity * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);
    }

   
    private void PatrolLogic(float distToPlayer)
    {
        if (distToPlayer < sightRange)
        {
            currentState = AIState.Chase;
            _giveUpTimer = 0f;
            return;
        }

        if (patrolPoints.Length == 0) return;
        Transform target = patrolPoints[_currentPatrolIndex];

        MoveTowards(target.position, patrolSpeed);

        float distToPoint = Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z),
                                             new Vector3(target.position.x, 0, target.position.z));

        if (distToPoint < 2.0f)
        {
            _waitTimer += Time.deltaTime;
            if (_waitTimer >= waitTime)
            {
                _currentPatrolIndex = (_currentPatrolIndex + 1) % patrolPoints.Length;
                _waitTimer = 0;
            }
        }
    }

    private void ChaseLogic(float distToPlayer)
    {
        MoveTowards(playerTarget.position, chaseSpeed);

        if (distToPlayer > losePlayerRange)
        {
            _giveUpTimer += Time.deltaTime;
            if (_giveUpTimer > 2.0f) // Chase for 2 extra seconds
            {
                currentState = AIState.Patrol;
                Debug.Log("Player escaped!");
            }
        }
        else
        {
            _giveUpTimer = 0;
        }

        if (distToPlayer <= attackRange)
        {
            currentState = AIState.Attack;
        }
    }

    private void AttackLogic(float distToPlayer)
    {
        Vector3 dir = (playerTarget.position - transform.position).normalized;
        dir.y = 0;
        if (dir != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 10f);

        if (distToPlayer > attackRange * 1.5f)
        {
            currentState = AIState.Chase;
        }
        else
        {
            if (PlayerHealth.Instance != null)
            {
                PlayerHealth.Instance.TakeDamage(10f * Time.deltaTime);
            }
        }
    }

    private void DistractedLogic(float distToPlayer)
    {
        if (distToPlayer < sightRange / 2f)
        {
            currentState = AIState.Chase;
            return;
        }

        MoveTowards(_distractionTarget, patrolSpeed);

        float distToStone = Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z),
                                             new Vector3(_distractionTarget.x, 0, _distractionTarget.z));

        if (distToStone < 1.0f)
        {
            _waitTimer += Time.deltaTime;
            if (_waitTimer >= distractionDuration)
            {
                currentState = AIState.Patrol;
                _waitTimer = 0;
            }
        }
    }

    public void Distract(Vector3 location)
    {
        if (currentState == AIState.Patrol || currentState == AIState.Distracted)
        {
            Debug.Log("Dino heard noise!");
            currentState = AIState.Distracted;
            _distractionTarget = location;
            _waitTimer = 0;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, losePlayerRange);

        Gizmos.color = Color.blue;

        Vector3 center = Application.isPlaying ? _spawnPosition : transform.position;
        Gizmos.DrawWireSphere(center, maxTerritoryRange);
    }
}
