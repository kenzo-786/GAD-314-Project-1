using UnityEngine;
using UnityEngine.AI;
using Unity.Collections;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]

public class DinoNavMeshAI : MonoBehaviour
{
    public enum DinoState { Patrol, Chase, Distracted, Attack }

    [Header("State")]
    public DinoState currentState;

    [Header("Settings")]
    public float patrolSpeed = 2.5f;
    public float chaseSpeed = 6.0f;
    public float sightRange = 15f;
    public float attackRange = 2.5f;
    public float losePlayerRange = 20f;

    [Header("Territory")]
    public float maxTerritoryRange = 40f;
    private Vector3 _spawnPosition;

    [Header("Patrol")]
    public Transform[] patrolPoints;
    public float waitAtPoint = 2.0f;

    [Header("Distraction")]
    public float distractionDuration = 5f;
    private Vector3 _distractionTarget;

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
        _spawnPosition = transform.position;

        if (playerTarget == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p) playerTarget = p.transform;
        }

        StartCoroutine(InitAgent());
    }

    private IEnumerator InitAgent()
    {
        yield return null;

        if (!_agent.enabled) _agent.enabled = true;

        if (_agent.isOnNavMesh)
        {

        }
        else
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(transform.position, out hit, 5.0f, NavMesh.AllAreas))
            {
                _agent.Warp(hit.position);
                Debug.Log($"[DinoAI] Snapped {name} to NavMesh.");
            }
            else
            {
                Debug.LogError($"[DinoAI] {name} could not find NavMesh within 5 units! Check your bake.");
                enabled = false;
                yield break;
            }
        }

        _agent.autoBraking = true;
        currentState = DinoState.Patrol;
        MoveToNextPatrolPoint();
    }

    private void Update()
    {
        if (playerTarget == null) return;

        float distToPlayer = Vector3.Distance(transform.position, playerTarget.position);
        float distToSpawn = Vector3.Distance(transform.position, _spawnPosition);

        if (distToSpawn > maxTerritoryRange && currentState != DinoState.Patrol)
        {
            currentState = DinoState.Patrol;
            MoveToNextPatrolPoint();
        }

        switch (currentState)
        {
            case DinoState.Patrol:
                PatrolLogic(distToPlayer);
                break;
            case DinoState.Chase:
                ChaseLogic(distToPlayer);
                break;
            case DinoState.Distracted:
                DistractedLogic(distToPlayer);
                break;
            case DinoState.Attack:
                AttackLogic(distToPlayer);
                break;
        }

        UpdateAnimations();

    }

    private void PatrolLogic(float distToPlayer)
    {
        _agent.speed = patrolSpeed;
        _agent.stoppingDistance = 0.1f;

        if (CanSeePlayer(distToPlayer))
        {
            currentState = DinoState.Chase;
            return;
        }

        if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
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
        _agent.stoppingDistance = attackRange - 0.5f;
        _agent.SetDestination(playerTarget.position);

        if (distToPlayer <= attackRange)
        {
            currentState = DinoState.Attack;
        }

        if (distToPlayer > losePlayerRange)
        {
            currentState = DinoState.Patrol;
            MoveToNextPatrolPoint();
        }
    }

    private void AttackLogic(float distToPlayer)
    {
        _agent.isStopped = true;

        Vector3 direction = (playerTarget.position - transform.position).normalized;
        direction.y = 0;
        if (direction != Vector3.zero)
        {
            Quaternion lookRot = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 5f);
        }

        if (distToPlayer > attackRange * 1.2f)
        {
            _agent.isStopped = false;
            currentState = DinoState.Chase;
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

    private void DistractedLogic(float distToPlayer)
    {
        if (distToPlayer < sightRange / 2f && CanSeePlayer(distToPlayer))
        {
            currentState = DinoState.Chase;
            return;
        }

        _agent.speed = patrolSpeed;
        _agent.stoppingDistance = 0.5f;
        _agent.SetDestination(_distractionTarget);

        if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
        {
            _waitTimer += Time.deltaTime;
            if (_waitTimer >= distractionDuration)
            {
                currentState = DinoState.Patrol;
                MoveToNextPatrolPoint();
                _waitTimer = 0f;
            }
        }
    }

    public void Distract(Vector3 location)
    {
        if (currentState == DinoState.Patrol || currentState == DinoState.Distracted)
        {
            currentState = DinoState.Distracted;
            _distractionTarget = location;
            _waitTimer = 0f;
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

        Gizmos.color = Color.blue;
        Vector3 center = Application.isPlaying ? _spawnPosition : transform.position;
        Gizmos.DrawWireSphere(center, maxTerritoryRange);
    }

}
