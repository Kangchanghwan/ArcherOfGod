using UnityEngine;

public class PlayerDeadState : PlayerState
{
    public PlayerDeadState(PlayerContext context, string animBoolName) : base(context, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        InputManagerSingleton.Instance.InputManager.Disable();
        rigidbody2D.simulated = false;
    }
}
