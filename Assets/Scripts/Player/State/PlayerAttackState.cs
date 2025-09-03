using UnityEngine;

public class PlayerAttackState: PlayerState
{
    private ArrowManager _arrowManager;

    public PlayerAttackState(PlayerContext context, string animBoolName, ArrowManager arrowManager) : base(context, animBoolName)
    {
        _arrowManager = arrowManager;
    }

    public override void Enter()
    {
        base.Enter();
        if(playerController.facingRight == false) playerController.Flip();
        
    }
    
    public override void Update()
    {
        base.Update();
        
        
        if (Mathf.Abs(player.xInput) > 0.1f)
        {
            playerController.ChangeState(playerController.MoveState);
        }

        if (triggerCalled)
        {
            playerController.ChangeState(playerController.CastingState);
        }
        
    }


    public void BezierShoot()
    {
        if (playerController.CurrentState == playerController.AttackState ||
            playerController.CurrentState == playerController.JumpShootState)
        {
            _arrowManager.BezierShoot(playerController.facingRight);
        }
    }
    
    public void LinearShoot()
    {
        if (playerController.CurrentState == playerController.AttackState ||
            playerController.CurrentState == playerController.JumpShootState)
        {
            _arrowManager.LinearShoot();
        }
    }
    
}
