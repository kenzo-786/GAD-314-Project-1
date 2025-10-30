using UnityEngine;
using UnityEngine.AI;

public class EnemyAIController : MonoBehaviour
{
    private enum AIState { Idle, Patrol, Chase, Attack }

    [Header("Patrol")]
    public Transform waypointsParent;
    public float waitTimeAtPoint = 2f;
    private int currentWaypoint = 0;
    private float waitTimer;

    [Header("Detection")]
    public float chaseRange = 10f;
    public float attackRange = 2f;

    [Header("Attack")]
    public float attackDamage = 10f;
    public float attackCooldown = 1f;
    private float lastAttackTime;

    private NavMeshAgent agent;
    private GameObject player;
    private PlayerHealth playerHealth;
    private AIState currentState;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            playerHealth = player.GetComponent<PlayerHealth>();

        currentState = AIState.Patrol;
        waitTimer = waitTimeAtPoint;

        if (waypointsParent && waypointsParent.childCount > 0)
            agent.SetDestination(waypointsParent.GetChild(currentWaypoint).position);
    }

    private void Update()
    {
        if (!player) return;
        float distance = Vector3.Distance(transform.position, player.transform.position);

        switch (currentState)
        {
            case AIState.Idle:
                HandleIdle(distance);
                break;
            case AIState.Patrol:
                HandlePatrol(distance);
                break;
            case AIState.Chase:
                HandleChase(distance);
                break;
            case AIState.Attack:
                HandleAttack(distance);
                break;
        }
    }

    private void HandleIdle(float distance)
    {
        agent.isStopped = true;
        if (waitTimer > 0)
        {
            waitTimer -= Time.deltaTime;
        }
        else
        {
            GoToNextWaypoint();
            currentState = AIState.Patrol;
        }
        if (distance <= chaseRange)
            currentState = AIState.Chase;
    }

    private void HandlePatrol(float distance)
    {
        agent.isStopped = false;
        if (!agent.pathPending && agent.remainingDistance <= 0.2f)
        {
            currentState = AIState.Idle;
            waitTimer = waitTimeAtPoint;
        }
        if (distance <= chaseRange)
            currentState = AIState.Chase;
    }

    private void HandleChase(float distance)
    {
        if (distance > attackRange)
        {
            agent.isStopped = false;
            agent.SetDestination(player.transform.position);
        }
        else
        {
            agent.isStopped = true;
            currentState = AIState.Attack;
        }
        if (distance > chaseRange)
            currentState = AIState.Patrol;
    }

    private void HandleAttack(float distance)
    {
        transform.LookAt(player.transform);
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            lastAttackTime = Time.time;
            if (playerHealth != null)
                playerHealth.TakeDamage(attackDamage);
        }
        if (distance > attackRange)
            currentState = AIState.Chase;
    }

    private void GoToNextWaypoint()
    {
        if (!waypointsParent || waypointsParent.childCount == 0) return;
        currentWaypoint = (currentWaypoint + 1) % waypointsParent.childCount;
        agent.SetDestination(waypointsParent.GetChild(currentWaypoint).position);
    }
}
