using UnityEngine;
using System.Collections.Generic;

public class MissionManager : MonoBehaviour
{
    public static MissionManager Instance;

    [Header("Configuration")]
    public GameObject missionEntryPrefab;
    public Transform missionListContainer;
    public AudioClip completeSound;
    public AudioSource audioSource;

    [Header("Icons")]
    public Sprite emptyCheckbox;
    public Sprite checkedCheckbox;

    private Dictionary<string, MissionEntry> activeMissions = new Dictionary<string, MissionEntry>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        if (!activeMissions.ContainsKey("teleport_lab"))
        {
            AddMission("teleport_lab", "Teleport to Mesozoic Era");
        }

    }

    public void AddMission(string id, string description)
    {
        if (activeMissions.ContainsKey(id)) return;

        if (missionListContainer == null) return;

        GameObject newMission = Instantiate(missionEntryPrefab, missionListContainer);
        MissionEntry entry = newMission.GetComponent<MissionEntry>();

        entry.Setup(description, emptyCheckbox, checkedCheckbox);

        activeMissions.Add(id, entry);
    }

    public void UpdateMission(string id, string newDescription)
    {
        if (activeMissions.ContainsKey(id))
        {
            activeMissions[id].UpdateText(newDescription);
        }
    }

    public void CompleteMission(string id)
    {
        if (activeMissions.ContainsKey(id))
        {
            activeMissions[id].SetComplete();

            if (completeSound && audioSource)
            {
                audioSource.PlayOneShot(completeSound);
            }
        }
    }
}
