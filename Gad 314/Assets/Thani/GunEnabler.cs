using UnityEngine;

public class GunEnabler : MonoBehaviour
{
    public NewShooting shooting;
    public GameObject gun;
    public bool isEnabled;

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Gun Pick-Up")
        {
            Destroy(other.gameObject);
            shooting.enabled = true;
            isEnabled = true;
            gun.SetActive(true);
        }
    }
}
