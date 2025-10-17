using UnityEngine;

public class MiniMapFollow : MonoBehaviour
{
    public Transform player;
    public float height = 30f;
    public bool rotateWithPlayer = true;

    void LateUpdate()
    {
        if (player == null) return;

        Vector3 newPos = player.position;
        newPos.y += height;
        transform.position = newPos;

        if (rotateWithPlayer)
            transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);
        else
            transform.rotation = Quaternion.Euler(90f, 0f, 0f);
    }
}