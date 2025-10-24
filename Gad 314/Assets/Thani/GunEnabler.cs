using UnityEngine;

public class GunEnabler : MonoBehaviour
{
    public Shooting shooting;
    public GameObject gun;

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Gun Pick-Up")
        {
            Destroy(other.gameObject);
            shooting.enabled = true;
            gun.SetActive(true);
        }
    }
}
