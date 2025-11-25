using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchProjectile : MonoBehaviour
{
    public GameObject projectile;
    public float launchVelocity = 30f;
    public float upforce;
    public Camera spearCamera;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("Threw a spear.");
            Throw();
        }
    }

    void Throw()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = spearCamera.farClipPlane;
        if(Physics.Raycast(spearCamera.ScreenPointToRay(mousePosition),out RaycastHit hit))
        {
            mousePosition.z = hit.distance;
        }
        Vector3 worldPosition = spearCamera.ScreenToWorldPoint(mousePosition);
        Vector3 direction = (worldPosition - transform.position).normalized;

        GameObject spear = Instantiate(projectile, transform.position, Quaternion.LookRotation(direction, Vector3.up));
        Rigidbody spearRB = spear.GetComponent<Rigidbody>();
        spearRB.useGravity = true;
        Vector3 force = direction * launchVelocity + Vector3.up * upforce;
        spearRB.AddForce(force, ForceMode.Impulse);
    }
}
