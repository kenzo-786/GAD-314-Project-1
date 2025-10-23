using Unity.VisualScripting;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public float bSpeed;
    public float firingRate, bDamage;

    public Transform bTransform;
    public GameObject bPrefab;

    private float timer;

    private void Update()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime / firingRate;
        }

        if(Input.GetButtonDown("Fire1") && timer <= 0)
        {
            Shoot();
            Debug.Log("He shoots!");
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bPrefab, bTransform.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().AddForce(bTransform.forward * bSpeed, ForceMode.Impulse);
        bullet.GetComponent<BulletBehavior>().pain = bDamage;
    }
}
