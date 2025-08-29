using UnityEngine;

public class EnemyJumpShootState : EnemyState
{
    [Header("Jump Settings")]
    public float jumpForce = 10f;
    
    public EnemyJumpShootState(StateMachine stateMachine, string animBoolName, Enemy enemy) 
        : base(stateMachine, animBoolName, enemy)
    {
    }
    
    public override void Enter()
    {
        base.Enter();
        enemy.canMove = false;
        enemy.SetVelocity(0,0);
    }
    
    public override void Update()
    {
        base.Update();
        if (enemy.ManualRotationActive())
        {
            FaceTarget();
        }

        if (triggerCalled)
        {
            stateMachine.ChangeState(enemy.moveState);
        }
        
    }
    
    public override void Exit()
    {
        base.Exit();
        enemy.canMove = true;
    }
}
