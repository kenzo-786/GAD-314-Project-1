using UnityEngine;

public class MiniMapCameraFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(0, 50, 0);

    void LateUpdate()
    {
        if (player == null) return;
        transform.position = new Vector3(player.position.x, player.position.y + offset.y, player.position.z);
    }
}