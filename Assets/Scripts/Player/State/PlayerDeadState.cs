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
        Rigidbody2D.simulated = false;
    }
}
