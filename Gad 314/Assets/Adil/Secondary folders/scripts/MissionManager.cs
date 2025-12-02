using UnityEngine;
using System.Collections.Generic;
using System.Collections;


public class MissionManager : MonoBehaviour
{
    public static MissionManager Instance;

    [Header("Configuration")]
    public GameObject missionEntryPrefab;
    public Transform missionListContainer;
    public AudioClip completeSound;
    public AudioSource audioSource;
    public float destroyDelay = 3.0f;

    [Header("Icons")]
    public Sprite emptyCheckbox;
    public Sprite checkedCheckbox;

    [System.Serializable]
    public class MissionData
    {
        public MissionEntry entryScript;
        public string description;
        public int currentProgress;
        public int targetAmount;
        public bool isComplete;
        public bool isRemoving;
    }

    private Dictionary<string, MissionData> activeMissions = new Dictionary<string, MissionData>();
    private int missionCounter = 0;

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

    public void AddMission(string id, string description, int targetAmount = 1)
    {
        if (activeMissions.ContainsKey(id)) return;
        if (missionListContainer == null) return;

        foreach (var key in activeMissions.Keys)
        {
            MissionData oldMission = activeMissions[key];

            if (oldMission.isComplete && !oldMission.isRemoving)
            {
                oldMission.isRemoving = true;
                StartCoroutine(RemoveMissionRoutine(key, oldMission.entryScript.gameObject));
            }
        }

            missionCounter++;

        GameObject newObj = Instantiate(missionEntryPrefab, missionListContainer);
        MissionEntry entry = newObj.GetComponent<MissionEntry>();

        string prefix = $"Mission {missionCounter}: ";
        string fullText = prefix + description;

        if (targetAmount > 1) fullText += $" (0/{targetAmount})";

        entry.Setup(fullText, emptyCheckbox, checkedCheckbox);

        MissionData data = new MissionData
        {
            entryScript = entry,
            description = prefix + description,
            currentProgress = 0,
            targetAmount = targetAmount,
            isComplete = false
        };

        activeMissions.Add(id, data);
        Debug.Log($"[Mission] Added: {id}");
    }

    public void AddProgress(string id, int amount)
    {
        if (!activeMissions.ContainsKey(id)) return;

        MissionData m = activeMissions[id];

        if (m.isComplete) return;

        m.currentProgress += amount;

        if (m.targetAmount > 1)
        {
            m.entryScript.UpdateText($"{m.description} ({m.currentProgress}/{m.targetAmount})");
        }

        if (m.currentProgress >= m.targetAmount)
        {
            CompleteMission(id);
        }
    }

    public void CompleteMission(string id)
    {
        if (activeMissions.ContainsKey(id))
        {
            MissionData m = activeMissions[id];

            if (!m.isComplete)
            {
                m.isComplete = true;

                m.currentProgress = m.targetAmount;

                m.entryScript.SetComplete();

                if (completeSound && audioSource)
                {
                    audioSource.PlayOneShot(completeSound);
                }

                Debug.Log($"[Mission] Completed: {id}");

              //  StartCoroutine(RemoveMissionRoutine(id, m.entryScript.gameObject));
            }
        }
    }

    private System.Collections.IEnumerator RemoveMissionRoutine(string id, GameObject uiObject)
    {
        yield return new WaitForSeconds(destroyDelay);

        if (uiObject != null)
            Destroy(uiObject);
    }
}
