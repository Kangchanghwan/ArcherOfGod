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
        Controller.CanMove = false;
    }
    
    public override void Update()
    {
        base.Update();
        // if (player.ManualRotationActive())
        // {
        //     FaceTarget();
        // }

        if (TriggerCalled)
        {
            Controller.ChangeState(Controller.CastingState);
        }
    }
    
    // protected void FaceTarget()
    // {
    //     if (player.target != null)
    //     {
    //         Vector3 directionToEnemy = player.target.GetTransform().position - player.transform.position;
    //         if (directionToEnemy.x > 0 && !playerController.facingRight)
    //             playerController.Flip();
    //     }
    // }
    
    public override void Exit()
    {
        base.Exit();
        Controller.CanMove = true;

    }
}