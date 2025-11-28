using UnityEngine;

public class TeleportSpawnPoint : MonoBehaviour
{
    public static Vector3 lastTeleportPosition;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) player.transform.position = lastTeleportPosition;
    }
}