using UnityEngine;

public class MissionZone : MonoBehaviour
{
    public enum ZoneType { AddMission, CompleteMission }

    [Header("Settings")]
    public ZoneType actionType;
    public string missionID;

    [Header("If Adding Mission")]
    public string description;
    private bool _hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (_hasTriggered) return;

        if (other.CompareTag("Player"))
        {
            Debug.Log($"[MissionZone] Player Detected in {gameObject.name}!");

            if (MissionManager.Instance != null)
            {
                if (actionType == ZoneType.AddMission)
                {
                    Debug.Log($"[MissionZone] Adding Mission: {missionID}");
                    MissionManager.Instance.AddMission(missionID, description);
                }
                else if (actionType == ZoneType.CompleteMission)
                {
                    Debug.Log($"[MissionZone] Completing Mission: {missionID}");
                    MissionManager.Instance.CompleteMission(missionID);
                }
                _hasTriggered = true;
            }
        }
    }
}