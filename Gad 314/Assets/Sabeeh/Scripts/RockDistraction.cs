using System;
using UnityEngine;

public class RockDistraction : MonoBehaviour
{
    public static event Action<Vector3> onRockThrown;

    public static void RockThrown(Vector3 position)
    {
        onRockThrown?.Invoke(position);
        Debug.Log("Rock noise at: " + position);
    }
    
}

