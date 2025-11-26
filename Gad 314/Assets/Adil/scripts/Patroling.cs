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

    public class AIController : MonoBehaviour
    {
        [Header("Patrol")]
        [SerializeField] private Transform wayPoints;
        [SerializeField] private float waitAtPoint = 2f;
        private int currentWayPoint;
        private float waitCounter;

        [Header("Components")]
        private NavMeshAgent agent;
        private Renderer rend;
        private Rigidbody rb;

        [Header("AI State")]
        [SerializeField] private AIState currentState;

        [Header("Chasing")]
        [SerializeField] private float chaseRange;

        [Header("Suspicious")]
        [SerializeField] private float suspiciousTime;
        private float lastSawPlayer;

        [Header("Noise")]
        public float hearRange = 30f;
        public float moveSpeed = 3f;
        private Vector3 targetPosition;
        private bool movingToNoise = false;

        private GameObject player;

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            rb = GetComponent<Rigidbody>();
            rend = GetComponent<Renderer>();
            player = GameObject.FindGameObjectWithTag("Player");

            currentWayPoint = 0;
            waitCounter = waitAtPoint;
            lastSawPlayer = suspiciousTime;

            agent.isStopped = false;
            agent.SetDestination(wayPoints.GetChild(currentWayPoint).position);

            // Lock physics rotation
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

            RockDistraction.onRockThrown += OnRockNoise;
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
                case AIState.Idle:
                    agent.isStopped = false;

                    if (waitCounter > 0)
                        waitCounter -= Time.deltaTime;
                    else
                    {
                        currentState = AIState.Patrolling;
                        agent.SetDestination(wayPoints.GetChild(currentWayPoint).position);
                    }

                    if (distanceToPlayer <= chaseRange)
                        currentState = AIState.Chasing;

                    if (movingToNoise)
                        currentState = AIState.Investigating;
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
                        currentState = AIState.Chasing;

                    if (movingToNoise)
                        currentState = AIState.Investigating;
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

                case AIState.Investigating:
                    agent.isStopped = true; // stop NavMeshAgent while investigating
                    MoveToNoise();

                    if (!movingToNoise)
                        currentState = AIState.Idle;

                    if (distanceToPlayer <= chaseRange)
                        currentState = AIState.Chasing;
                    break;
            }
        }

        void MoveToNoise()
        {
            if (!movingToNoise) return;

            Vector3 direction = targetPosition - transform.position;
            direction.y = 0;

            Vector3 move = direction.normalized * moveSpeed * Time.deltaTime;
            rb.MovePosition(rb.position + move);

            if (direction != Vector3.zero)
            {
                Quaternion rot = Quaternion.LookRotation(direction);
                rb.MoveRotation(Quaternion.Slerp(rb.rotation, rot, 0.15f));
            }

            if (Vector3.Distance(transform.position, targetPosition) < 0.8f)
                movingToNoise = false;
        }

        void OnRockNoise(Vector3 pos)
        {
            if (Vector3.Distance(transform.position, pos) <= hearRange)
            {
                targetPosition = pos;
                movingToNoise = true;
                Highlight(); // optional visual feedback
            }
        }

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
}
    