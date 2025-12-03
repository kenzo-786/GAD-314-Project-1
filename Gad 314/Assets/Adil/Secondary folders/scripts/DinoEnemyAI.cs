using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class DinoEnemyAI : MonoBehaviour
{
    public enum State { Patrol, Chase, Distracted, Attack }

    [Header("Current State")]
    public State currentState;

    [Header("Senses")]
    public float sightRange = 20f;
    public float attackRange = 3f;
    public Transform playerTarget;

    [Header("Patrol Settings")]
    public Transform[] patrolPoints;
    public float patrolSpeed = 3.5f;
    public float chaseSpeed = 7.0f;
    public float waitTime = 2f;

    [Header("Distraction")]
    public float distractionDuration = 5f;

    private NavMeshAgent _agent;
    private int _currentPatrolIndex;
    private float _waitTimer;


    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();

        if (playerTarget == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p) playerTarget = p.transform;

            currentState = State.Patrol;
            GoToNextPatrolPoint();
        }
    }

    private void Update()
    {
        if (playerTarget == null) return;

        if (GameManager.Instance && !GameManager.Instance.CanMove())
        {
            if (!_agent.isStopped) _agent.isStopped = true;
            return;
        }
        _agent.isStopped = false;

        float distanceToPlayer = Vector3.Distance(transform.position, playerTarget.position);

        switch (currentState)
        {
            case State.Patrol:
                PatrolLogic(distanceToPlayer);
                break;
            case State.Chase:
                ChaseLogic(distanceToPlayer);
                break;
            case State.Distracted:
                DistractedLogic(distanceToPlayer);
                break;
            case State.Attack:
                AttackLogic(distanceToPlayer);
                break;
        }
    }

    private void PatrolLogic(float distToPlayer)
    {
        _agent.speed = patrolSpeed;

        if (distToPlayer < sightRange)
        {
            currentState = State.Chase;
            return;
        }

        if (!_agent.pathPending && _agent.remainingDistance < 0.5f)
        {
            _waitTimer += Time.deltaTime;
            if (_waitTimer >= waitTime)
            {
                GoToNextPatrolPoint();
                _waitTimer = 0;
            }
        }
    }

    private void ChaseLogic(float distToPlayer)
    {
        _agent.speed = chaseSpeed;
        _agent.SetDestination(playerTarget.position);

        if (distToPlayer <= attackRange)
        {
            currentState = State.Attack;
        }

        if (distToPlayer > sightRange * 1.5f)
        {
            currentState = State.Patrol;
            GoToNextPatrolPoint(); 
        }
    }

    private void AttackLogic(float distToPlayer)
    {
        if (distToPlayer > attackRange)
        {
            currentState = State.Chase;
            _agent.isStopped = false;
        }     
    }

    private void DistractedLogic(float distToPlayer)
    {
        _agent.speed = patrolSpeed;

        if (distToPlayer < sightRange / 2f)
        {
            currentState = State.Chase;
            return;
        }

        if (!_agent.pathPending && _agent.remainingDistance < 0.5f)
        {
            _waitTimer += Time.deltaTime;
            if (_waitTimer >= distractionDuration)
            {
                currentState = State.Patrol;
                GoToNextPatrolPoint();
                _waitTimer = 0;
            }
        }
    }

    private void GoToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;
        _agent.destination = patrolPoints[_currentPatrolIndex].position;
        _currentPatrolIndex = (_currentPatrolIndex + 1) % patrolPoints.Length;
    }

    public void Distract(Vector3 location)
    {
        if (currentState == State.Patrol || currentState == State.Distracted)
        {
            Debug.Log("Noise detected! Investigating...");
            currentState = State.Distracted;
            _agent.SetDestination(location);
            _waitTimer = 0;
        }
    }
}
