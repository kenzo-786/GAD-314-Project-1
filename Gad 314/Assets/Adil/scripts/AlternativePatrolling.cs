using UnityEngine;
using UnityEngine.AI;

public class AlternativePatrolling : MonoBehaviour
{
    [Header("Patrol")]
    [SerializeField] private Transform wayPoints;
    private int currentWayPoint = 0;

    [Header("Component")]
    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(wayPoints.GetChild(currentWayPoint).position);
    }

    private void Update()
    {
        if (!agent.pathPending && agent.remainingDistance <= 0.2f)
        {
            currentWayPoint = (currentWayPoint + 1) % wayPoints.childCount;
            agent.SetDestination(wayPoints.GetChild(currentWayPoint).position);
        }
    }
}

