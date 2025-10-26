using UnityEngine;

public class Crouching : NewPlayerBase
{
    public override void EnterState(NewPlayerMovement movement)
    {
        
    }

    public override void UpdateState(NewPlayerMovement movement)
    {
        if (Input.GetKey(KeyCode.LeftShift)) ExitState(movement, movement.Run);
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (movement.direc.magnitude < 0.1f) ExitState(movement, movement.Idle);
            else ExitState(movement, movement.Walk);
        }

        if (movement.vertiInput < 0) movement.moveSpeed = movement.crouchBackSpeed;
        else movement.moveSpeed = movement.crouchSpeed;
    }

    void ExitState(NewPlayerMovement movement, NewPlayerBase state)
    {
        movement.SwitchState(state);
    }
}
