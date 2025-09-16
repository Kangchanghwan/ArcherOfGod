using UnityEngine;

public class IdleState : StateBase
{
    protected override string GetAnimationName() => "Idle";

    public override void Enter()
    {
        base.Enter();
        Rigidbody2D.simulated = false;
    }
}