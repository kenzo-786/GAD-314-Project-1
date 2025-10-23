using UnityEngine;

public class SimplePlayerSwitcher : MonoBehaviour
{
    public PlayerMovement player1;
    public PlayerMovement player2;

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
                SetActivePlayer(player2);
            else
                SetActivePlayer(player1);
        }

        // Smoothly move camera toward active player
        Vector3 targetPos = activePlayer.transform.position + activePlayer.transform.TransformDirection(cameraOffset);
        cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetPos, Time.deltaTime * followSpeed);
        cameraTransform.LookAt(activePlayer.transform.position + Vector3.up * 1.5f);
    }

    void SetActivePlayer(PlayerMovement newPlayer)
    {
        player1.isActive = false;
        player2.isActive = false;

        activePlayer = newPlayer;
        activePlayer.isActive = true;
    }
}