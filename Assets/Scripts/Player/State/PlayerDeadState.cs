using UnityEngine;

public class PlayerDeadState : PlayerState
{

    public override void Enter()
    {
        base.Enter();
        Rigidbody2D.simulated = false;
    }
}
