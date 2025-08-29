using UnityEngine;

public class EnemyCastingState: EnemyState
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public EnemyCastingState(StateMachine stateMachine, string animBoolName, Enemy enemy) : base(stateMachine, animBoolName, enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }
    
    public override void Update()
    {
        base.Update();
        

        if (enemy.skillManager.jumpShoot.CanUseSkill())
        {
            stateMachine.ChangeState(enemy.jumpShootState);
            enemy.skillManager.jumpShoot.SetSillOnCooldown();
        }

        if (triggerCalled)
        {
            stateMachine.ChangeState(enemy.attackState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
