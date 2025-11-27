using System;
using UnityEngine;

public class RockCollisionHelper : MonoBehaviour
{
    // Event for enemies to listen to
    public static event Action<Vector3> onRockThrown;

    // Call this when a rock is thrown
    public static void RockThrown(Vector3 position)
    {
        onRockThrown?.Invoke(position);
    }
}

