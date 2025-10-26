using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Collections;


public class AlternativePatrolling : MonoBehaviour
{
    enum AIState
    {
        Idle, Patrolling, Chasing
    }

    [Header("Patrol Settings")]
    [SerializeField] private Transform wayPoints;
    [SerializeField] private float waitAtPoint = 2f;
    private int currentWayPoint;
    private float waitCounter;

    [Header("Chase Settings")]
    [SerializeField] private float chaseRange = 5f;
    [SerializeField] private float suspiciousTime = 3f;
    private float lastSawPlayerTimer;

    [Header("Components")]
    private NavMeshAgent agent;
    private GameObject player;

    [Header("Debug State")]
    [SerializeField] private AIState currentState = AIState.Patrolling;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");

        currentWayPoint = 0;
        waitCounter = waitAtPoint;
        lastSawPlayerTimer = 0f;

        agent.isStopped = false;
        agent.SetDestination(wayPoints.GetChild(currentWayPoint).position);
    }

    private void Update()
    {
        if (player == null || agent == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        switch (currentState)
        {
            case AIState.Idle:
                HandleIdleState(distanceToPlayer);
                break;

            case AIState.Patrolling:
                HandlePatrollingState(distanceToPlayer);
                break;

            case AIState.Chasing:
                HandleChasingState(distanceToPlayer);
                break;
        }
    }

    private void HandleIdleState(float distanceToPlayer)
    {
        agent.isStopped = true;

        waitCounter -= Time.deltaTime;
        if (waitCounter <= 0f)
        {
            currentState = AIState.Patrolling;
            agent.isStopped = false;
            agent.SetDestination(wayPoints.GetChild(currentWayPoint).position);
        }

        if (distanceToPlayer <= chaseRange)
        {
            currentState = AIState.Chasing;
            agent.isStopped = false;
        }
    }

    private void HandlePatrollingState(float distanceToPlayer)
    {
        agent.isStopped = false;

        if (!agent.pathPending && agent.remainingDistance <= 0.2f)
        {
            currentWayPoint = (currentWayPoint + 1) % wayPoints.childCount;
            currentState = AIState.Idle;
            waitCounter = waitAtPoint;
        }

        if (distanceToPlayer <= chaseRange)
        {
            currentState = AIState.Chasing;
        }
    }

    private void HandleChasingState(float distanceToPlayer)
    {
        agent.isStopped = false;
        agent.SetDestination(player.transform.position);

        if (distanceToPlayer > chaseRange)
        {
            lastSawPlayerTimer += Time.deltaTime;
            if (lastSawPlayerTimer >= suspiciousTime)
            {
                currentState = AIState.Idle;
                lastSawPlayerTimer = 0f;
            }
        }
        else
        {
            lastSawPlayerTimer = 0f; // reset if player is still in range
        }
    }
}



