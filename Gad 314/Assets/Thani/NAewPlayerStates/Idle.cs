using UnityEngine;

public class Idle : NewPlayerBase
{
    public override void EnterState(NewPlayerMovement movement)
    {

    }

    public override void UpdateState(NewPlayerMovement movement)
    {
        if (movement.direc.magnitude > 0.1f)
        {
            if (Input.GetKey(KeyCode.LeftShift)) movement.SwitchState(movement.Run);
            else movement.SwitchState(movement.Walk);
        }
        if (Input.GetKeyDown(KeyCode.C)) movement.SwitchState(movement.Crouch);
    }
}
