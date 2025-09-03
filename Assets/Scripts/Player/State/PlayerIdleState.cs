using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(PlayerContext context, string animBoolName) : base(context, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        rigidbody2D.simulated = false;
    }
}
