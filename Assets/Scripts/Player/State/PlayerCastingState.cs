using UnityEngine;

public class PlayerCastingState : PlayerState
{

    public override void Enter()
    {
        base.Enter();
        Rigidbody2D.linearVelocity = Vector2.zero;
    }
}
