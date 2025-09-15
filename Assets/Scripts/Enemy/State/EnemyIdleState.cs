using UnityEngine;

public class EnemyIdleState : EnemyState
{
    protected override string GetAnimationName() => "Idle";

    public override void Enter()
    {
        base.Enter();
        Rigidbody2D.simulated = false;
    }
}