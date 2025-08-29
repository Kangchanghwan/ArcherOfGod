using UnityEngine;

public class PlayerMoveState : PlayerState
{
    public PlayerMoveState(StateMachine stateMachine, string animBoolName, Player player) 
        : base(stateMachine, animBoolName, player)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }
    
    public override void Update()
    {
        base.Update();

        if (player.canMove == false) return; 

        player.SetVelocity(player.xInput *  player.moveSpeed * Time.deltaTime, rigidbody2D.linearVelocity.y);

        if (Mathf.Abs(player.xInput) < 0.1f)
        {
            stateMachine.ChangeState(player.castingState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
