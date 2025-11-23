using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float health = 100;
    public BulletBehavior bullet;
    public AudioSource hit;

    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            bullet = other.gameObject.GetComponent<BulletBehavior>();
            Debug.Log("Enemy hit!");
            health -= bullet.pain;
            Debug.Log(health);
            hit.Play();
            Destroy(other.gameObject);
            Debug.Log("Bullet destroyed!");
        }
    }
}
