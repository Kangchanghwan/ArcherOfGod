using UnityEngine;

public class PlayerDeadState : PlayerState
{
    protected override string GetAnimationName() => "Dead";

    public override void Enter()
    {
        base.Enter();
        Rigidbody2D.simulated = false;
    }
}