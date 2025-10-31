using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public BulletBehavior bulletDamage;
    public int health = 100;

    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Bullet")
        {
            bulletDamage = other.gameObject.GetComponent<BulletBehavior>();
            bulletDamage.pain -= health;
            Destroy(other.gameObject);
        }
    }
}
