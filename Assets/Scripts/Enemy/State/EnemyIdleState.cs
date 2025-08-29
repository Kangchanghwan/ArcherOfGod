using UnityEngine;

public class EnemyIdleState : EnemyState
{

    
    public EnemyIdleState(StateMachine stateMachine, string animBoolName, Enemy enemy) 
        : base(stateMachine, animBoolName, enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
        enemy.rb.simulated = false;
    }
}
