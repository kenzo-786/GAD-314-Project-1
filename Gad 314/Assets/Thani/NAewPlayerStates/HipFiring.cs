using UnityEngine;

public class HipFiring : NewPlayerAimBase
{
    public override void EnterState(NewShooting aim)
    {

    }

    public override void UpdateState(NewShooting aim)
    {
        if (Input.GetKey(KeyCode.Mouse1)) aim.SwitchState(aim.Aim);
    }
}
