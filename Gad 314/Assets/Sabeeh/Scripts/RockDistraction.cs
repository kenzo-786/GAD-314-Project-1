using System;
using UnityEngine;

public class RockDistraction : MonoBehaviour
{
    public static Action<Vector3> onRockThrown;

    public static void Trigger(Vector3 pos)
    {
        onRockThrown?.Invoke(pos);
    }
}
