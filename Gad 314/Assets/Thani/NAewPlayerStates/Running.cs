using UnityEngine;

public class Running : NewPlayerBase
{
    public override void EnterState(NewPlayerMovement movement)
    {

    }

    public override void UpdateState(NewPlayerMovement movement)
    {
        if (Input.GetKeyUp(KeyCode.LeftShift)) ExitState(movement, movement.Walk);
        else if (movement.direc.magnitude < 0.1f) ExitState(movement, movement.Idle);

        if (movement.vertiInput < 0) movement.moveSpeed = movement.runBackSpeed;
        else movement.moveSpeed = movement.runSpeed;
    }

    void ExitState(NewPlayerMovement movement, NewPlayerBase state)
    {
        movement.SwitchState(state);
    }
}
