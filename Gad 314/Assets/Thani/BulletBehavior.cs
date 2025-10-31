using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    public float pain;
    public float secondsToLive = 1;
    public EnemyHealth enemy;

    private void Update()
    {
        secondsToLive -= Time.deltaTime;

        if(secondsToLive < 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Terrain")
        {
            Debug.Log("Hit the level!");
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            enemy = other.gameObject.GetComponent<EnemyHealth>();
            Debug.Log("Hit an enemy!");
            pain -= enemy.health;
            Destroy(gameObject);
        }
    }
}
