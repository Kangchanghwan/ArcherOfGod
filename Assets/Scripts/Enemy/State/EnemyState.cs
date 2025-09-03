using UnityEngine;

public class EnemyState : EntityState
{

    protected Enemy enemy;
    protected EnemySkillManager skillManager;

    
    public EnemyState(StateMachine stateMachine, string animBoolName, Enemy enemy) : 
        base(stateMachine, animBoolName)
    {
        this.enemy = enemy;
        animator = enemy.animator;
        rigidbody2D = enemy.rb;
        skillManager = enemy.skillManager;
    }
    
    public override void Update()
    {
        base.Update();
        

        if (CanJumpShot())
        {
            skillManager.jumpShoot.SetSillOnCooldown();
            stateMachine.ChangeState(enemy.jumpShootState);
        }
        
    }
    
    private bool CanJumpShot()
    {
        if (skillManager.jumpShoot.CanUseSkill() == false)
        {
            return false;
        }
        if (enemy.stateMachine.currentState == enemy.jumpShootState)
        {
            return false;
        }
        if (enemy.stateMachine.currentState == enemy.idleState)
        {
            return false;
        }
        if (enemy.stateMachine.currentState == enemy.deadState)
        {
            return false;
        }
        return true;
    }
    
    protected void FaceTarget()
    {
        if (enemy.target != null)
        {
            Vector3 directionToEnemy = enemy.target.GetTransform().position - enemy.transform.position;
            if (directionToEnemy.x > 0 && !enemy.facingRight)
                enemy.Flip();
            else if (directionToEnemy.x < 0 && enemy.facingRight)
                enemy.Flip();
        }
    }

}
