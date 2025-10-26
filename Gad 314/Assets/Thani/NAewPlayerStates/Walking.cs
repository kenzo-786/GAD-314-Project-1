using UnityEngine;

public class Walking : NewPlayerBase
{
    public override void EnterState(NewPlayerMovement movement)
    {

    }

    public override void UpdateState(NewPlayerMovement movement)
    {
        if (Input.GetKey(KeyCode.LeftShift)) ExitState(movement, movement.Run);
        else if (Input.GetKeyDown(KeyCode.C)) ExitState(movement, movement.Crouch);
        else if (movement.direc.magnitude < 0.1f) ExitState(movement, movement.Idle);

        if (movement.vertiInput < 0) movement.moveSpeed = movement.walkBackSpeed;
        else movement.moveSpeed = movement.walkSpeed;
    }

    void ExitState(NewPlayerMovement movement, NewPlayerBase state)
    {
        movement.SwitchState(state);
    }
}
