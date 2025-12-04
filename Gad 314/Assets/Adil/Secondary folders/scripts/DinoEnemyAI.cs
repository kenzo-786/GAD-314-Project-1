using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(CharacterController))]

public class DinoEnemyAI : MonoBehaviour
{
    public enum AIState { Patrol, Chase, Distracted, Attack }

    [Header("Current State")]
    public AIState currentState;

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

    [Header("Physics")]
    public float gravity = -20f;

    private CharacterController _controller;
    private int _currentPatrolIndex;
    private float _waitTimer;
    private Vector3 _velocity;
    private Vector3 _distractionTarget;

    private void Start()
    {
        if (playerTarget == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p) playerTarget = p.transform;
        }

        currentState = AIState.Patrol;
    }

    private void Update()
    {
        ApplyGravity();

        if (playerTarget == null) return;

        if (GameManager.Instance && !GameManager.Instance.CanMove()) return;

        float distanceToPlayer = Vector3.Distance(transform.position, playerTarget.position);

        switch (currentState)
        {
            case AIState.Patrol:
                PatrolLogic(distanceToPlayer);
                break;
            case AIState.Chase:
                ChaseLogic(distanceToPlayer);
                break;
            case AIState.Distracted:
                DistractedLogic(distanceToPlayer);
                break;
            case AIState.Attack:
                AttackLogic(distanceToPlayer);
                break;
        }
    }

    private void MoveTowards(Vector3 targetPosition, float speed)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            Quaternion lookRot = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 5f);

            _controller.Move(direction * speed * Time.deltaTime);
        }
    }

    private void ApplyGravity()
    {
        if (_controller.isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }

        _velocity.y += gravity * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);
    }

    private void PatrolLogic(float distToPlayer)
    {

        if (distToPlayer < sightRange)
        {
            currentState = AIState.Chase;
            return;
        }

        if (patrolPoints.Length == 0) return;

        Transform target = patrolPoints[_currentPatrolIndex];
        MoveTowards(target.position, patrolSpeed);

        float distToPoint = Vector3.Distance(
           new Vector3(transform.position.x, 0, transform.position.z),
           new Vector3(target.position.x, 0, target.position.z));

        if (distToPoint < 1.0f)
        {
            _waitTimer += Time.deltaTime;
            if (_waitTimer >= waitTime)
            {
                _currentPatrolIndex = (_currentPatrolIndex + 1) % patrolPoints.Length;
                _waitTimer = 0;
            }
        }
    }

    private void ChaseLogic(float distToPlayer)
    {
        MoveTowards(playerTarget.position, chaseSpeed);

        if (distToPlayer <= attackRange)
        {
            currentState = AIState.Attack;
        }

        if (distToPlayer > sightRange * 1.5f)
        {
            currentState = AIState.Patrol;
        }
    }

    private void AttackLogic(float distToPlayer)
    {
        Vector3 direction = (playerTarget.position - transform.position).normalized;
        direction.y = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 10f);

        if (distToPlayer > attackRange)
        {
            currentState = AIState.Chase;
        }
    }

    private void DistractedLogic(float distToPlayer)
    {
        if (distToPlayer < sightRange / 2f)
        {
            currentState = AIState.Chase;
            return;
        }

        MoveTowards(_distractionTarget, patrolSpeed);

        float distToStone = Vector3.Distance(
           new Vector3(transform.position.x, 0, transform.position.z),
           new Vector3(_distractionTarget.x, 0, _distractionTarget.z));

        if (distToStone < 1.0f)
        {
            _waitTimer += Time.deltaTime;
            if (_waitTimer >= distractionDuration)
            {
                currentState = AIState.Patrol;
                _waitTimer = 0;
            }
        }
    }

    public void Distract(Vector3 location)
    {
        if (currentState == AIState.Patrol || currentState == AIState.Distracted)
        {
            Debug.Log("Noise detected! Moving to investigate.");
            currentState = AIState.Distracted;
            _distractionTarget = location;
            _waitTimer = 0;
        }
    }

}
