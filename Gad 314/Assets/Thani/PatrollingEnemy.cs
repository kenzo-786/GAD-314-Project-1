using UnityEngine;

public class PatrollingEnemy : MonoBehaviour
{
    public Transform[] points;
    public int currentTargetPoint;
    public float speed;
    public bool playerSpotted;

    void Start()
    {
        currentTargetPoint = 0;
    }

    void Update()
    {
        if(playerSpotted == false)
        {
            if(transform.position == points[currentTargetPoint].position)
            {
                NextPoint();
            }
            transform.position = Vector3.MoveTowards(transform.position, points[currentTargetPoint].position, speed * Time.deltaTime);
        }
        if (playerSpotted == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, points[currentTargetPoint].position, speed * Time.deltaTime);
        }
    }

    void NextPoint()
    {
        currentTargetPoint++;
        if(currentTargetPoint >= points.Length)
        {
            currentTargetPoint = 0;
        }
    }
}
