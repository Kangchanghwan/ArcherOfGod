using UnityEngine;

public class PlayerCastingState : PlayerState
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public PlayerCastingState(StateMachine stateMachine, string animBoolName, Player player) : base(stateMachine, animBoolName, player)
    {
    }

    public override void Enter()
    {
        base.Enter();
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
            stateMachine.ChangeState(player.attackState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
