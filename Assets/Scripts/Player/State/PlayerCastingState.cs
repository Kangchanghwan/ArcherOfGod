using UnityEngine;

public class PlayerCastingState : PlayerState
{
    protected override string GetAnimationName() => "Casting";

    public override void Enter()
    {
        base.Enter();
        Rigidbody2D.linearVelocity = Vector2.zero;
    }
}