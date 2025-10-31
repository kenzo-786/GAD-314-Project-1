using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    public float pain;
    public float secondsToLive = 1;

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
        Destroy(gameObject);
    }
}
