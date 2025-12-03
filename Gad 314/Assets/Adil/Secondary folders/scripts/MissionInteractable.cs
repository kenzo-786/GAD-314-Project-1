using UnityEngine;

public class MissionInteractable : MonoBehaviour
{
    [Header("Mission Logic")]
    public string missionToComplete;
    public string missionToStart;
    public string nextMissionDesc;

    [Header("Interaction")]
    public float holdDuration = 1.0f;
    public bool disableAfterUse = true;

    [Header("Unlockable")]
    public GameObject objectToEnable;
    public GameObject objectToDisable;

    private bool _inRange;
    private float _timer;
    private bool _used = false;

    private void Update()
    {
        if (_used) return;

        if (_inRange && Input.GetKey(KeyCode.E))
        {
            _timer += Time.deltaTime;
            if (InteractionHUD.Instance) InteractionHUD.Instance.UpdateProgress(_timer / holdDuration);

            if (_timer >= holdDuration) Complete();
        }
        else
        {
            if (_timer > 0)
            {
                _timer = 0;
                if (InteractionHUD.Instance) InteractionHUD.Instance.UpdateProgress(0);
            }
        }
    }

    private void Complete()
    {
        _used = true;
        if (InteractionHUD.Instance) InteractionHUD.Instance.Hide();

        if (MissionManager.Instance)
        {
            if (!string.IsNullOrEmpty(missionToComplete))
                MissionManager.Instance.AddProgress(missionToComplete, 1);

            if (!string.IsNullOrEmpty(missionToStart))
                MissionManager.Instance.AddMission(missionToStart, nextMissionDesc, 1);
        }

        PetController pet = GetComponent<PetController>();
        if (pet != null)
        {
            pet.Repair();
        }

        if (objectToEnable != null)
        {
            objectToEnable.SetActive(true);
        }

        if (objectToDisable != null)
        {
            objectToDisable.SetActive(false);
        }

            if (disableAfterUse) this.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_used && other.CompareTag("Player"))
        {
            _inRange = true;
            if (InteractionHUD.Instance) { InteractionHUD.Instance.Show(); InteractionHUD.Instance.UpdateProgress(0); }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _inRange = false;
            _timer = 0;
            if (InteractionHUD.Instance) InteractionHUD.Instance.Hide();
        }
    }
}
