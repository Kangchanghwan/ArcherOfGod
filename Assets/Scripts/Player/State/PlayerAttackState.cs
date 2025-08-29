using UnityEngine;

public class PlayerAttackState: PlayerState
{
    
    public PlayerAttackState(StateMachine stateMachine, string animBoolName, Player player) : base(stateMachine, animBoolName, player)
    {
    }
    
    public override void Enter()
    {
        base.Enter();
        if(player.facingRight == false) player.Flip();
        
    }
    
    public override void Update()
    {
        base.Update();
        
        
        if (Mathf.Abs(player.xInput) > 0.1f)
        {
            stateMachine.ChangeState(player.moveState);
        }

        if (triggerCalled)
        {
            stateMachine.ChangeState(player.castingState);
        }
        
    }

    public override void Exit()
    {
        base.Exit();
    }
    
}
