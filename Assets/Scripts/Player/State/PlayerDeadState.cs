using UnityEngine;

public class PlayerDeadState : PlayerState
{
    public PlayerDeadState(StateMachine stateMachine, string animBoolName, Player player) 
        : base(stateMachine, animBoolName, player)
    {
    }

    public override void Enter()
    {
        base.Enter();

        input.Disable();
        player.rb.simulated = false;
    }
}
