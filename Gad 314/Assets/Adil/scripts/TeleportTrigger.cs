using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportTrigger : MonoBehaviour
{
    [Header("Settings")]
    public string missionToComplete = "teleport_lab";
    public string sceneToLoad = "DinoSceneName";


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (MissionManager.Instance != null)
            {
                MissionManager.Instance.CompleteMission(missionToComplete);
            }

            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
