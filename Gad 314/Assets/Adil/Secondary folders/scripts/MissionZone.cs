using UnityEngine;

public class MissionZone : MonoBehaviour
{
    public enum ZoneType { AddMission, CompleteMission }

    [Header("Settings")]
    public ZoneType actionType;
    public string missionID;

    [Header("If Adding Mission")]
    public string description;
    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (actionType == ZoneType.AddMission)
        {
            MissionManager.Instance.AddMission(missionID, description);
        }
        else if (actionType == ZoneType.CompleteMission)
        {
            MissionManager.Instance.CompleteMission(missionID);
        }

        hasTriggered = true;
    }


}
