using UnityEngine;

public class PlayerIdleState : PlayerState
{
    protected override string GetAnimationName() => "Idle";

    public override void Enter()
    {
        base.Enter();
        Rigidbody2D.simulated = false;
    }
}
