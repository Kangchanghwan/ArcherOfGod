using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(StateMachine stateMachine, string animBoolName, Player player) 
        : base(stateMachine, animBoolName, player)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.rb.simulated = false;
    }
}
