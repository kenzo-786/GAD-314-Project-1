using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.AI;


public class Patroling : MonoBehaviour
{
    enum AIState
    {
        Idle, Patrolling, Chasing
    }

    [Header("Patrol")]
    [SerializeField] private Transform wayPoints;
    [SerializeField] private float waitAtPoint = 2f;
    private int currentWayPoint;
    private float waitCounter;

    [Header("Component")]
    NavMeshAgent agent;


    [Header("AI State")]
    [SerializeField] AIState currentState;

    [Header("chasing")]
    [SerializeField] private float chaseRange;

    [Header("Suspicious")]
    [SerializeField] private float suspiciousTime;
    private float lastSawPlayer;

    private GameObject player;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");

        currentWayPoint = 0;
        waitCounter = waitAtPoint;
        lastSawPlayer = suspiciousTime;

        agent.isStopped = false;
        agent.SetDestination(wayPoints.GetChild(currentWayPoint).position);

        Debug.Log(wayPoints.childCount);
       // currentState = AIState.Patrolling;


    }

    private void Update()
    {
        Debug.Log(currentState + " | " + agent.isStopped + " | " + agent.remainingDistance);



        if (!agent.pathPending && agent.remainingDistance <= 0.2f)
        {
            currentWayPoint = (currentWayPoint + 1) % wayPoints.childCount;
            agent.SetDestination(wayPoints.GetChild(currentWayPoint).position);
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        switch (currentState)
        {
            case AIState.Idle:

                if(waitCounter > 0)
                {
                    waitCounter -= Time.deltaTime;
                }
                else
                {
                    currentState = AIState.Patrolling;
                    agent.isStopped = false;
                    agent.SetDestination(wayPoints.GetChild(currentWayPoint).position);
                }
                if(distanceToPlayer <= chaseRange)
                {
                    currentState = AIState.Chasing;
                }
                break;


            case AIState.Patrolling:

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
                break;

            case AIState.Chasing:

                agent.isStopped = false;
                agent.SetDestination(player.transform.position);

                if (distanceToPlayer > chaseRange)
                {
                    lastSawPlayer -= Time.deltaTime;

                    if (lastSawPlayer <= 0)
                    {
                        currentState = AIState.Idle;
                        lastSawPlayer = suspiciousTime;
                    }
                }
                else
                {
                    
                    lastSawPlayer = suspiciousTime;
                }

                break;

        }
    }
}
