using UnityEngine;

public class NewShooting2 : MonoBehaviour
{
    public float firingRate;
    float firingRateTimer;
    public bool semiAuto;

    public GameObject bullet;
    public Transform barrelPos;
    public float bulletVelocity;
    public int bulletsPerShot;
    NewShooting aim;

    public int bulletsLeft;

    void Start()
    {
        aim = GetComponentInParent<NewShooting>();
        firingRateTimer = firingRate;
    }

    void Update()
    {
        if(ShouldFire()) Fire();
    }

    bool ShouldFire()
    {
        firingRateTimer += Time.deltaTime;
        if (firingRateTimer < firingRate) return false;
        if (semiAuto && Input.GetKeyDown(KeyCode.Mouse0)) return true;
        if (semiAuto! && Input.GetKey(KeyCode.Mouse0)) return true;
        return false;
    }

    void Fire()
    {
        firingRateTimer = 0;
        barrelPos.LookAt(aim.aimPos);
        for (int i = 0; i < bulletsPerShot; i++)
        {
            GameObject currentBullet = Instantiate(bullet, barrelPos.position, barrelPos.rotation);
            Rigidbody rb = currentBullet.GetComponent<Rigidbody>();
            rb.AddForce(barrelPos.forward * bulletVelocity, ForceMode.Impulse);
        }
        bulletsLeft--;
    }
}
