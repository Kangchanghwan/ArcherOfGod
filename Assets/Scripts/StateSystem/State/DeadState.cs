using UnityEngine;

public class DeadState : StateBase
{
    protected override string GetAnimationName() => "Dead";

    public override void Enter()
    {
        base.Enter();
        Rigidbody2D.simulated = false;
    }
}