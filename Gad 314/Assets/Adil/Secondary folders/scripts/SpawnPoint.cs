using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public string spawnID;

    public string missionToAdd_ID = "";
    public string missionToAdd_Desc = "";

    private void Start()
    {
        if (SpawnPointManager.TargetSpawnID == spawnID)
        {
            SpawnPointManager.TargetSpawnID = "";

            if (!string.IsNullOrEmpty(missionToAdd_ID))
            {
                if (MissionManager.Instance != null)
                {
                    Debug.Log($"[SpawnPoint] Adding Mission: {missionToAdd_Desc}");
                    MissionManager.Instance.AddMission(missionToAdd_ID, missionToAdd_Desc);
                }
                else
                {
                    Debug.LogWarning("[SpawnPoint] Cannot add mission - MissionManager missing!");
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
        Gizmos.DrawRay(transform.position, transform.forward * 2);
    }
}
