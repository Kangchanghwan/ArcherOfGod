using UnityEngine;

public class EnemyDeadState : EnemyState
{
    protected override string GetAnimationName() => "Dead";

    public override void Enter()
    {
        base.Enter();
        Rigidbody2D.simulated = false;
    }
}