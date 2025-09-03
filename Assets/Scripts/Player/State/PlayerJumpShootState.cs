using UnityEngine;

public class PlayerJumpShootState : PlayerState
{
    [Header("Jump Settings")]
    public float jumpForce = 10f;


    public PlayerJumpShootState(PlayerContext context, string animBoolName, float jumpForce) : base(context, animBoolName)
    {
        this.jumpForce = jumpForce;
    }

    public override void Enter()
    {
        base.Enter();
        playerController.canMove = false;
    }
    
    public override void Update()
    {
        base.Update();
        if (player.ManualRotationActive())
        {
            FaceTarget();
        }

        if (triggerCalled)
        {
            playerController.ChangeState(playerController.CastingState);
        }
    }
    
    public override void Exit()
    {
        base.Exit();
        playerController.canMove = true;

    }
}