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
    private Dictionary<string, int> pendingProgress = new Dictionary<string, int>();

    private Dictionary<string, KeyValuePair<string, string>> missionChains = new Dictionary<string, KeyValuePair<string, string>>();

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
        string startID = SanitizeID("teleport_lab");
        if (!activeMissions.ContainsKey(startID))
        {
            AddMission(startID, "Teleport to Mesozoic Era");
        }

        LinkMission("collect_resources", "return_lab", "Return to the Lab Portal");

        LinkMission("return_lab", "craft_cure", "Craft the Time Cure at the Workbench");
    }

    private void LinkMission(string doneID, string nextID, string nextDesc)
    {
        string id = SanitizeID(doneID);
        if (!missionChains.ContainsKey(id))
        {
            missionChains.Add(id, new KeyValuePair<string, string>(SanitizeID(nextID), nextDesc));
        }
    }

    private string SanitizeID(string input)
    {
        if (string.IsNullOrEmpty(input)) return "";
        return input.Trim().ToLower();
    }

    public void AddMission(string rawID, string description, int targetAmount = 1)
    {
        string id = SanitizeID(rawID);

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

        int startProgress = 0;
        if (pendingProgress.ContainsKey(id))
        {
            startProgress = pendingProgress[id];
            Debug.Log($"[Mission] {id} starting with pending progress: {startProgress}");
            pendingProgress.Remove(id);
        }

        string prefix = $"Mission {missionCounter}: ";
        string fullText = prefix + description;

        if (targetAmount > 1) fullText += $" ({startProgress}/{targetAmount})";

        entry.Setup(fullText, emptyCheckbox, checkedCheckbox);

        MissionData data = new MissionData
        {
            entryScript = entry,
            description = prefix + description,
            currentProgress = startProgress,
            targetAmount = targetAmount,
            isComplete = false,
            isRemoving = false
        };

        activeMissions.Add(id, data);
        Debug.Log($"[Mission] Added: {id}");

        if (data.currentProgress >= data.targetAmount)
        {
            CompleteMission(id);
        }

    }

    public void AddProgress(string rawID, int amount)
    {
        string id = SanitizeID(rawID);

        if (activeMissions.ContainsKey(id))
        {
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
        else
        {
            if (pendingProgress.ContainsKey(id))
            {
                pendingProgress[id] += amount;
            }
            else
            {
                pendingProgress.Add(id, amount);
            }
            Debug.Log($"[Mission] Progress saved for future mission '{id}': {pendingProgress[id]}");
        }
    }

    public void CompleteMission(string rawID)
    {
        string id = SanitizeID(rawID);


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

                if (missionChains.ContainsKey(id))
                {
                    KeyValuePair<string, string> nextInfo = missionChains[id];
                    StartCoroutine(StartNextMissionRoutine(nextInfo.Key, nextInfo.Value));
                }
            }
        }
    }

    public bool IsMissionComplete(string rawID)
    {
        string id = SanitizeID(rawID);
        if (activeMissions.ContainsKey(id))
        {
            return activeMissions[id].isComplete;
        }

        return false;
    }

    public bool IsMissionActive(string rawID)
    {
        string id = SanitizeID(rawID);

        if (activeMissions.ContainsKey(id))
        {
            return !activeMissions[id].isComplete;
        }
        return false;
    }

    private IEnumerator StartNextMissionRoutine(string nextID, string nextDesc)
    {
        yield return new WaitForSeconds(2.0f);
        AddMission(nextID, nextDesc);
    }

    private System.Collections.IEnumerator RemoveMissionRoutine(string id, GameObject uiObject)
    {
        yield return new WaitForSeconds(destroyDelay);

        if (uiObject != null)
            Destroy(uiObject);
    }
}
