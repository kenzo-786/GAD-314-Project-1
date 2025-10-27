using UnityEngine;

public class SimplePlayerSwitcher : MonoBehaviour
{
    public PlayerMovement player1;
    public PlayerMovement player2;

    public float swapRange = 5f; // Maximum distance for swapping
    public float player2FollowSpeed = 3f; // Speed at which Player 2 follows Player 1

    private PlayerMovement activePlayer;
    private Transform cameraTransform;
    private Vector3 cameraOffset = new Vector3(0, 3, -7);
    private float followSpeed = 5f;

    void Start()
    {
        cameraTransform = Camera.main.transform;
        SetActivePlayer(player1);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (activePlayer == player1)
            {
                float distance = Vector3.Distance(player1.transform.position, player2.transform.position);
                if (distance <= swapRange)
                {
                    SetActivePlayer(player2);
                }
                else
                {
                    Debug.Log("Player 2 is too far to switch!");
                }
            }
            else
            {
                SetActivePlayer(player1);
            }
        }

        // Smoothly move camera toward active player
        Vector3 targetPos = activePlayer.transform.position + activePlayer.transform.TransformDirection(cameraOffset);
        cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetPos, Time.deltaTime * followSpeed);
        cameraTransform.LookAt(activePlayer.transform.position + Vector3.up * 1.5f);

        // Player 2 follows Player 1 if Player 1 is active
        if (activePlayer == player1 && player2 != null)
        {
            float step = player2FollowSpeed * Time.deltaTime;
            float distance = Vector3.Distance(player1.transform.position, player2.transform.position);
            if (distance > 1f) // Keep a small distance
            {
                player2.transform.position = Vector3.MoveTowards(player2.transform.position, player1.transform.position, step);
            }
        }
    }

    void SetActivePlayer(PlayerMovement newPlayer)
    {
        player1.isActive = false;
        player2.isActive = false;

        activePlayer = newPlayer;
        activePlayer.isActive = true;
    }

    void OnDrawGizmosSelected()
    {
        if (player1 != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(player1.transform.position, swapRange);
        }
    }
}