using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MissionLogScrollingList : MonoBehaviour
{
    [SerializeField] private GameObject contentParent;
    [SerializeField] private GameObject missionLogButtonPrefab;

    private Dictionary<string, MissionLogButton> idToButtonMap = new Dictionary<string, MissionLogButton>();

    private void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            MissionInfoSO missionInfoTest = ScriptableObject.CreateInstance<MissionInfoSO>();
        }
    }

    public MissionLogButton CreateButtonIfNotExists(Mission mission, UnityAction selectAction)
    {
        MissionLogButton missionLogButton = null;

        if (!idToButtonMap.ContainsKey(mission.info.id))
        {
            missionLogButton = InstantiateMissionLogButton(mission, selectAction);
        }
        else
        {
            missionLogButton = idToButtonMap[mission.info.id];
        }
        return missionLogButton;
    }

    private MissionLogButton InstantiateMissionLogButton(Mission mission, UnityAction selectAction)
    {
        MissionLogButton missionLogButton = Instantiate(missionLogButtonPrefab, contentParent.transform).GetComponent<MissionLogButton>();
        missionLogButton.gameObject.name = mission.info.id + "_button";
        missionLogButton.Initialize(mission.info.displayName, selectAction);
        idToButtonMap[mission.info.id] = missionLogButton;
        return missionLogButton;
    }
}
