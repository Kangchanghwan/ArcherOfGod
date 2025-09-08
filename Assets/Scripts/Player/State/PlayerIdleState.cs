using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public override void Enter()
    {
        base.Enter();
        Rigidbody2D.simulated = false;
    }
}
