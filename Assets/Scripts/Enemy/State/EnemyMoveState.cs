using UnityEngine;

public class EnemyMoveState : EnemyState
{

    
    public EnemyMoveState(StateMachine stateMachine, string animBoolName, Enemy enemy) 
        : base(stateMachine, animBoolName, enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }
    
    public override void Update()
    {
        base.Update();

        if (enemy.canMove == false) return; 

        // enemy.SetVelocity(enemy.xInput *  enemy.moveSpeed * Time.deltaTime, rigidbody2D.linearVelocity.y);
        //
        // if (Mathf.Abs(enemy.xInput) < 0.1f)
        // {
        //     stateMachine.ChangeState(enemy.castingState);
        // }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
