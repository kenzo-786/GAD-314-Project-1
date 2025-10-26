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

    //-----------------------------------//

    public float bSpeed;
    public float firingRate, bDamage;
    public int bAmount = 0;

    public Transform bTransform;
    public GameObject bPrefab;

    private float timer;

    void Start()
    {
        SwitchState(Hip);
    }

    void Update()
    {
        xAxis += Input.GetAxisRaw("Mouse X") * mouseSensitivity;
        yAxis -= Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
        yAxis = Mathf.Clamp(yAxis, -80, 80);

        curState.UpdateState(this);

        if (timer > 0)
        {
            timer -= Time.deltaTime / firingRate;
        }

        if (Input.GetButtonDown("Fire1") && timer <= 0 && bAmount > 0)
        {
            Shoot();
            Debug.Log("He shoots!");
            bAmount--;
            Debug.Log("Bullets left: " + bAmount);
        }
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

    void Shoot()
    {
        GameObject bullet = Instantiate(bPrefab, bTransform.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().AddForce(bTransform.forward * bSpeed, ForceMode.Impulse);
        bullet.GetComponent<BulletBehavior>().pain = bDamage;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Bullet Pick-Up")
        {
            bAmount = 10;
            Destroy(other.gameObject);
            Debug.Log("Picked up 10 ammo!");
        }
    }
}
