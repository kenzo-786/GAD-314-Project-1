using UnityEngine;

public class Aiming : NewPlayerAimBase
{
    public override void EnterState(NewShooting aim)
    {

    }

    public override void UpdateState(NewShooting aim)
    {
        if (Input.GetKeyUp(KeyCode.Mouse1)) aim.SwitchState(aim.Hip);
    }
}
