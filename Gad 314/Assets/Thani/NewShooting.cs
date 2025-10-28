 using UnityEngine;
using Unity.Cinemachine;

public class NewShooting : MonoBehaviour
{
    NewPlayerAimBase curState;
    public HipFiring Hip = new HipFiring();
    public Aiming Aim = new Aiming();

    public float mouseSensitivity = 1;
    public float xAxis, yAxis;
    public Transform camFollowPos;

    public CinemachineCamera vCam;
    public float adsFov = 40;
    public float hipFov;
    public float currentFov;
    public float fovSmoothSpeed = 10;

    public Transform aimPos;
    public Vector3 actualAimPos;
    public float aimSpeed = 20;
    public LayerMask aimMask;

    void Start()
    {
        vCam = GetComponentInChildren<CinemachineCamera>();
        hipFov = vCam.Lens.FieldOfView;
        SwitchState(Hip);
    }

    void Update()
    {
        xAxis += Input.GetAxisRaw("Mouse X") * mouseSensitivity;
        yAxis -= Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
        yAxis = Mathf.Clamp(yAxis, -80, 80);

        vCam.Lens.FieldOfView = Mathf.Lerp(vCam.Lens.FieldOfView, currentFov, fovSmoothSpeed * Time.deltaTime);

        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, aimMask))
        {
            aimPos.position = Vector3.Lerp(aimPos.position, hit.point, aimSpeed * Time.deltaTime);
        }

        curState.UpdateState(this);
    }

    private void LateUpdate()
    {
        camFollowPos.localEulerAngles = new Vector3(yAxis, camFollowPos.localEulerAngles.y, camFollowPos.localEulerAngles.z);
        transform.eulerAngles = new Vector3(camFollowPos.localEulerAngles.x, xAxis, camFollowPos.localEulerAngles.z);
    }

    public void SwitchState(NewPlayerAimBase state)
    {
        curState = state;
        curState.EnterState(this);
    }
}
