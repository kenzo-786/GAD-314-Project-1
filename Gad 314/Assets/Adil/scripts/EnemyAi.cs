using UnityEngine;
using UnityEngine.AI;


public class EnemyAi : MonoBehaviour
{

    public NavMeshAgent navMeshAgent;
    public float startTime = 3;
    public float timeToRotate = 2;
    public float dash = 6;
    public float sprint = 9;

    public float viewRadius = 15;
    public float viewAngle = 90;
    public LayerMask playerMask;
    public LayerMask obstacleMask;
    public float meshRes = 1f;
    public int edge = 4;
    public float edgeDistance = 0.5f;

    public Transform[] wayPoints;
    int currentWayPointIndex;

    Vector3 playerLastPos = Vector3.zero;
    Vector3 playerPos;

    float waitTime;
    float timeRotation;
    bool playerInRange;
    bool playerNear;
    bool isPartol;
    bool caughtPlayer;

 
    void Start()
    {
        playerPos = Vector3.zero;
        isPartol = true;    
        caughtPlayer = false;
        playerInRange = false;
        waitTime = startTime;
        timeRotation = timeToRotate;

        currentWayPointIndex = 0;
        navMeshAgent = GetComponent<NavMeshAgent>();

        navMeshAgent.isStopped = false;
        navMeshAgent.speed = dash;
        navMeshAgent.SetDestination(wayPoints[currentWayPointIndex].position);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CaughtPlayer()
    {
        caughtPlayer = true;
    }

    void Move(float speed)
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speed;
    }

    void Stop()
    {
        navMeshAgent.isStopped =true;
        navMeshAgent.speed = 0;
    }

    public void NextPoint()
    {

    }
    void LookingPlayer(Vector3 player)
    {
        navMeshAgent.SetDestination(player);
        if( Vector3.Distance(transform.position, player) < 0.3)
        {
            if( waitTime <= 0)
            {
                playerNear = false;
                Move(dash);
                navMeshAgent.SetDestination(wayPoints[currentWayPointIndex].position);
                waitTime = startTime;
                timeRotation = timeToRotate;
            }
            else
            {
                Stop();
                waitTime -= Time.deltaTime;
            }
        }
    }

}
