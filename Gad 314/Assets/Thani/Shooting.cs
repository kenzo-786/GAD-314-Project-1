using Unity.VisualScripting;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public float bSpeed;
    public float firingRate, bDamage;
    public int bAmount = 0;

    public Transform bTransform;
    public GameObject bPrefab;

    [SerializeField] Transform aimPos;
    [SerializeField] float aimSpeed = 20;
    [SerializeField] LayerMask aimMask;

    private float timer;

    private void Update()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime / firingRate;
        }

        if(Input.GetButtonDown("Fire1") && timer <= 0 && bAmount > 0)
        {
            Shoot();
            Debug.Log("He shoots!");
            bAmount--;
            Debug.Log("Bullets left: " + bAmount);
        }

        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);

        if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, aimMask))
        {
            aimPos.position = Vector3.Lerp(aimPos.position, hit.point, aimSpeed * Time.deltaTime);
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bPrefab, bTransform.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().AddForce(bTransform.forward * bSpeed, ForceMode.Impulse);
        bullet.GetComponent<BulletBehavior>().pain = bDamage;
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Bullet Pick-Up")
        {
            bAmount = 10;
            Destroy(other.gameObject);
            Debug.Log("Picked up 10 ammo!");
        }
    }
}
