using UnityEngine;
using Unity.Cinemachine;

public class NewShooting : MonoBehaviour
{
    private NewPlayerAimBase curState;
    public HipFiring Hip = new HipFiring();
    public Aiming Aim = new Aiming();
    public AudioSource shootSFX;

    [Header("Look")]
    [Range(0.05f, 10f)] public float mouseSensitivity = 1f;
    public float xAxis, yAxis;
    public Transform camFollowPos;

    [Header("Camera")]
    public CinemachineCamera vCam;          
    public Camera gameplayCam;              
    public float adsFov = 40f;
    [HideInInspector] public float hipFov;
    [HideInInspector] public float currentFov;
    public float fovSmoothSpeed = 10f;

    [Header("Aim")]
    public Transform aimPos;               
    public Vector3 actualAimPos;
    public float aimSpeed = 20f;
    public LayerMask aimMask = ~0;          
    public float maxAimDistance = 2000f;

    [Header("Firing")]
    public Transform muzzle;                
    public bool useHitscan = true;          
    public float hitscanRange = 2000f;
    public int bAmount = 0;

    [Header("Projectile")]
    public Rigidbody projectilePrefab;
    public float projectileSpeed = 120f;

    public LineRenderer tracer;            
    public float tracerDuration = 0.03f;

    public GunEnabler enabler;

    void Start()
    {
        if (vCam == null) vCam = GetComponentInChildren<CinemachineCamera>();
        if (gameplayCam == null) gameplayCam = Camera.main;

        if (vCam != null)
        {
            hipFov = vCam.Lens.FieldOfView;
            currentFov = hipFov;
        }
        SwitchState(Hip);
    }

    void Update()
    {
        xAxis += Input.GetAxisRaw("Mouse X") * mouseSensitivity;
        yAxis -= Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
        yAxis = Mathf.Clamp(yAxis, -80f, 80f);

        if (vCam != null)
            vCam.Lens.FieldOfView = Mathf.Lerp(vCam.Lens.FieldOfView, currentFov, fovSmoothSpeed * Time.deltaTime);

        Ray aimRay = GetAimRay();
        if (Physics.Raycast(aimRay, out RaycastHit hit, maxAimDistance, aimMask, QueryTriggerInteraction.Ignore))
        {
            Vector3 target = hit.point;
            aimPos.position = Vector3.Lerp(aimPos.position, target, aimSpeed * Time.deltaTime);
            actualAimPos = target;
        }
        else
        {
            Vector3 fallback = aimRay.GetPoint(maxAimDistance);
            aimPos.position = Vector3.Lerp(aimPos.position, fallback, aimSpeed * Time.deltaTime);
            actualAimPos = fallback;
        }

        // === Input: Fire ===
        if (Input.GetButtonDown("Fire1"))
        {
            if (enabler.isEnabled == true)
            {
                Debug.Log("He shoots!");
                bAmount--;
                Debug.Log("Bullets left: " + bAmount);
                shootSFX.Play();
                if (useHitscan) FireHitscan();
                else FireProjectile();
            }
        }

        if (curState != null) curState.UpdateState(this);
    }

    void LateUpdate()
    {
        if (camFollowPos != null)
        {
            camFollowPos.localEulerAngles = new Vector3(
                yAxis, camFollowPos.localEulerAngles.y, camFollowPos.localEulerAngles.z
            );
        }

        transform.rotation = Quaternion.Euler(0f, xAxis, 0f);
    }

    public void SwitchState(NewPlayerAimBase state)
    {
        curState = state;
        curState?.EnterState(this);
    }

    public void SetADS(bool adsOn)
    {
        currentFov = adsOn ? adsFov : hipFov;
    }

    Ray GetAimRay()
    {
        Camera cam = gameplayCam != null ? gameplayCam : Camera.main;
        Vector3 mp = Input.mousePosition;
        return cam.ScreenPointToRay(mp);
    }

    void FireHitscan()
    {
        if (muzzle == null)
        {
            Debug.LogWarning("[NewShooting] Missing muzzle Transform.");
            return;
        }

        Ray ray = GetAimRay();

        Vector3 endPoint = ray.GetPoint(hitscanRange);
        if (Physics.Raycast(ray, out RaycastHit hit, hitscanRange, aimMask, QueryTriggerInteraction.Ignore))
        {
            endPoint = hit.point;
        }

        if (tracer != null)
        {
            StartCoroutine(DoTracer(muzzle.position, endPoint));
        }
    }

    void FireProjectile()
    {
        if (muzzle == null || projectilePrefab == null)
        {
            Debug.LogWarning("[NewShooting] Missing muzzle or projectile prefab.");
            return;
        }

        Vector3 dir = (actualAimPos - muzzle.position).normalized;
        Rigidbody rb = Instantiate(projectilePrefab, muzzle.position, Quaternion.LookRotation(dir));
        rb.linearVelocity = dir * projectileSpeed;
    }

    System.Collections.IEnumerator DoTracer(Vector3 a, Vector3 b)
    {
        tracer.enabled = true;
        tracer.positionCount = 2;
        tracer.SetPosition(0, a);
        tracer.SetPosition(1, b);
        yield return new WaitForSeconds(tracerDuration);
        tracer.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bullet Pick-Up")
        {
            bAmount = 10;
            Destroy(other.gameObject);
            Debug.Log("Picked up 10 ammo!");
        }
    }
}
