using UnityEngine;

public class EnemyAttackState : EnemyState
{
    public EnemyAttackState(StateMachine stateMachine, string animBoolName, Enemy enemy) : base(stateMachine,
        animBoolName, enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
        if (enemy.facingRight) enemy.Flip();
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled)
        {
            stateMachine.ChangeState(enemy.moveState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
