using System.Collections;
using UnityEngine;

public class Tp : MonoBehaviour
{
    [Header("Teleport Setup")]
    [SerializeField] Transform transformTp; 
    [SerializeField] GameObject player;
    [SerializeField] float teleportDelay = 0.2f;
    [SerializeField] bool oneWay = true;     

    private bool isTeleporting = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player && !isTeleporting)
        {
            StartCoroutine(Teleport());
        }
    }

    IEnumerator Teleport()
    {
        isTeleporting = true;

        yield return new WaitForSeconds(teleportDelay);

        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        player.transform.position = transformTp.position;

        if (cc != null) cc.enabled = true;

    
        Tp destinationPortal = transformTp.GetComponent<Tp>();
        if (oneWay && destinationPortal != null)
        {
            destinationPortal.DisableTemporarily();
        }

        isTeleporting = false;
    }


    public void DisableTemporarily()
    {
        StartCoroutine(DisableForSeconds(1f)); 
    }

    IEnumerator DisableForSeconds(float duration)
    {
        Collider col = GetComponent<Collider>();
        col.enabled = false;
        yield return new WaitForSeconds(duration);
        col.enabled = true;
    }
}

