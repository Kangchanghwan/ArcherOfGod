using UnityEngine;

public class PlayerJumpShootState : PlayerState
{
    [Header("Jump Settings")]
    public float jumpForce = 10f;
    
    public PlayerJumpShootState(StateMachine stateMachine, string animBoolName, Player player) 
        : base(stateMachine, animBoolName, player)
    {
    }
    
    public override void Enter()
    {
        base.Enter();
        player.canMove = false;
        player.SetVelocity(0,0);
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
            stateMachine.ChangeState(player.castingState);
        }
    }
    
    public override void Exit()
    {
        base.Exit();
        player.canMove = true;

    }
}