using UnityEngine;

public class EnemyAttackState: EnemyState
{
    
    public EnemyAttackState(StateMachine stateMachine, string animBoolName, Enemy enemy) : base(stateMachine, animBoolName, enemy)
    {
    }
    
    public override void Enter()
    {
        base.Enter();
        if(enemy.facingRight) enemy.Flip();
        
    }
    
    public override void Update()
    {
        base.Update();
        
        // if (Mathf.Abs(enemy.xInput) > 0.1f)
        // {
        //     stateMachine.ChangeState(enemy.moveState);
        // }

        if (triggerCalled)
        {
            stateMachine.ChangeState(enemy.castingState);
        }
        
    }

    public override void Exit()
    {
        base.Exit();
    }
    
}
