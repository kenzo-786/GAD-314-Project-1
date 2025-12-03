using UnityEngine;
using System.Collections;

public class PlayerSpawnPoint : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(MovePlayerToSpawn());
    }

    private IEnumerator MovePlayerToSpawn()
    {
        yield return null;

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            CharacterController cc = player.GetComponent<CharacterController>();
            if (cc) cc.enabled = false;

            player.transform.position = transform.position;
            player.transform.rotation = transform.rotation;

            if (cc) cc.enabled = true;

            Debug.Log($"[SpawnPoint] Moved Player to {gameObject.name}");
        }
        else
        {
            Debug.LogWarning("[SpawnPoint] Could not find object with tag 'Player'!");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 2f);
    }
}
