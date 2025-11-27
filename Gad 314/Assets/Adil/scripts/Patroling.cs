using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.AI;


public class Patroling : MonoBehaviour
{
    public enum AIState
    {
        Idle, Patrolling, Chasing, Investigating
    }

   
    
        [Header("Patrol")]
        public Transform wayPoints;
        public float waitAtPoint = 2f;
        private int currentWayPoint = 0;
        private float waitCounter;

        [Header("Components")]
        private NavMeshAgent agent;
        private Renderer rend;

        [Header("AI State")]
        public AIState currentState;

        [Header("Chasing")]
        public float chaseRange;

        [Header("Noise")]
        public float hearRange = 30f;

        private Vector3 targetPosition;
        private GameObject player;

        private bool investigatingRock = false;

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            rend = GetComponent<Renderer>();
            player = GameObject.FindGameObjectWithTag("Player");

            waitCounter = waitAtPoint;

            RockDistraction.onRockThrown += OnRockNoise;

            // Start patrolling
            currentState = AIState.Patrolling;
            agent.SetDestination(wayPoints.GetChild(currentWayPoint).position);
        }

        void OnDestroy()
        {
            RockDistraction.onRockThrown -= OnRockNoise;
        }

        void Update()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            switch (currentState)
            {
                case AIState.Patrolling:
                    HandlePatrol();
                    if (distanceToPlayer <= chaseRange)
                        currentState = AIState.Chasing;
                    break;

                case AIState.Chasing:
                    agent.isStopped = false;
                    agent.SetDestination(player.transform.position);
                    if (distanceToPlayer > chaseRange)
                        currentState = investigatingRock ? AIState.Investigating : AIState.Patrolling;
                    break;

                case AIState.Investigating:
                    agent.isStopped = false;
                    agent.SetDestination(targetPosition);

                    // Reached rock, go back to patrol
                    if (Vector3.Distance(transform.position, targetPosition) < 0.5f)
                    {
                        investigatingRock = false;
                        currentState = AIState.Patrolling;
                        SetNextPatrolPoint();
                    }

                    if (distanceToPlayer <= chaseRange)
                        currentState = AIState.Chasing;
                    break;
            }
        }

        void HandlePatrol()
        {
            if (!agent.pathPending && agent.remainingDistance < 0.2f)
            {
                waitCounter -= Time.deltaTime;
                if (waitCounter <= 0f)
                {
                    SetNextPatrolPoint();
                }
            }
        }

        void SetNextPatrolPoint()
        {
            currentWayPoint = (currentWayPoint + 1) % wayPoints.childCount;
            agent.SetDestination(wayPoints.GetChild(currentWayPoint).position);
            waitCounter = waitAtPoint;
        }

        void OnRockNoise(Vector3 pos)
        {
            if (Vector3.Distance(transform.position, pos) <= hearRange)
            {
                targetPosition = pos;
                investigatingRock = true;
                currentState = AIState.Investigating;

                StartCoroutine(FlashRed());
            }
        }

        IEnumerator FlashRed()
        {
            Color original = rend.material.color;
            rend.material.color = Color.red;
            yield return new WaitForSeconds(0.3f);
            rend.material.color = original;
        }
    }
