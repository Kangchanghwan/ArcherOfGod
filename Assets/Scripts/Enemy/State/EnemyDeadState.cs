using UnityEngine;

public class EnemyDeadState : EnemyState
{

    
    public EnemyDeadState(StateMachine stateMachine, string animBoolName, Enemy enemy) 
        : base(stateMachine, animBoolName, enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();

        enemy.rb.simulated = false;
    }
}
