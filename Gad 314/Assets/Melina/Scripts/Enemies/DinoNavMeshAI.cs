using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]

public class DinoNavMeshAI : MonoBehaviour
{
    public enum AIState { Patrol, Chase, Attack }

    [Header("State")]
    public AIState currentState;

    [Header("Settings")]
    public float patrolSpeed = 2.5f;
    public float chaseSpeed = 6.0f;
    public float sightRange = 15f;
    public float attackRange = 2.5f;
    public float losePlayerRange = 20f;

    [Header("Patrol")]
    public Transform[] patrolPoints;
    public float waitAtPoint = 2.0f;

    [Header("References")]
    public Transform playerTarget;
    public LayerMask obstacleMask;

    private NavMeshAgent _agent;
    private Animator _animator;
    private int _patrolIndex = 0;
    private float _waitTimer = 0f;
    private float _attackCooldown = 0f;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();

        if (playerTarget == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p) playerTarget = p.transform;

            _agent.speed = patrolSpeed;
            _agent.stoppingDistance = attackRange - 0.5f;

            currentState = AIState.Patrol;
            MoveToNextPatrolPoint();
        }
    }

    private void Update()
    {
        if (playerTarget == null) return;

        float distToPlayer = Vector3.Distance(transform.position, playerTarget.position);

        switch (currentState)
        {
            case AIState.Patrol:
                PatrolLogic(distToPlayer);
                break;
            case AIState.Chase:
                ChaseLogic(distToPlayer);
                break;
            case AIState.Attack:
                AttackLogic(distToPlayer);
                break;
        }

        UpdateAnimations();
    }

    private void PatrolLogic(float distToPlayer)
    {
        _agent.speed = patrolSpeed;

        if (CanSeePlayer(distToPlayer))
        {
            currentState = AIState.Chase;
            return;
        }

        if (!_agent.pathPending && _agent.remainingDistance < 0.5f)
        {
            _waitTimer += Time.deltaTime;
            if (_waitTimer >= waitAtPoint)
            {
                MoveToNextPatrolPoint();
                _waitTimer = 0f;
            }
        }
    }

    private void ChaseLogic(float distToPlayer)
    {
        _agent.speed = chaseSpeed;
        _agent.SetDestination(playerTarget.position);

        if (distToPlayer <= attackRange)
        {
            currentState = AIState.Attack;
        }

        if (distToPlayer > losePlayerRange)
        {
            currentState = AIState.Patrol;
            MoveToNextPatrolPoint();
        }
    }

    private void AttackLogic(float distToPlayer)
    {
        _agent.isStopped = true;
        transform.LookAt(new Vector3(playerTarget.position.x, transform.position.y, playerTarget.position.z));

        if (distToPlayer > attackRange * 1.2f)
        {
            _agent.isStopped = false;
            currentState = AIState.Chase;
            return;
        }

        _attackCooldown -= Time.deltaTime;
        if (_attackCooldown <= 0)
        {
            _animator.SetTrigger("Attack");
            _attackCooldown = 2.0f;

            if (PlayerHealth.Instance)
            {
                PlayerHealth.Instance.TakeDamage(10);
            }
        }
    }

    private void MoveToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;

        _agent.destination = patrolPoints[_patrolIndex].position;
        _patrolIndex = (_patrolIndex + 1) % patrolPoints.Length;
    }

    private bool CanSeePlayer(float dist)
    {

        if (dist > sightRange) return false;

        Vector3 dirToPlayer = (playerTarget.position - transform.position).normalized;

        if (!Physics.Raycast(transform.position + Vector3.up, dirToPlayer, dist, obstacleMask))
        {
            return true;
        }
        return false;
    }

    private void UpdateAnimations()
    {
        float speed = _agent.velocity.magnitude / chaseSpeed;
        _animator.SetFloat("Speed", speed, 0.1f, Time.deltaTime);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

}
