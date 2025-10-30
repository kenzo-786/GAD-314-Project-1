
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dashing : MonoBehaviour
{
    AlternativeMovement movementScript;

    public float dashSpeed;
    public float dashTime;


     void Start()
    {
        movementScript = GetComponent<AlternativeMovement>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(Dash());
        }
    }

    IEnumerator Dash()
    {
        float startTime = Time.time;
        
        while(Time.time < startTime + dashTime) 
        { 
            movementScript.GetComponent<CharacterController>().Move(movementScript.moveDir * dashSpeed * Time.deltaTime);

            yield return null;
        }

    }

}
