using UnityEngine;
using UnityEngine.UI;

public class MiniMapPlayerIcon : MonoBehaviour
{
    public Transform player;           
    public RectTransform icon;         
    public float mapScale = 1f;        

    void Update()
    {
        if (player == null || icon == null) return;
        
        Vector3 playerPos = player.position;
        icon.anchoredPosition = new Vector2(playerPos.x, playerPos.z) * mapScale;
        
        icon.localRotation = Quaternion.Euler(0, 0, -player.eulerAngles.y);
    }
}