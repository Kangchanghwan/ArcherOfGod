using UnityEngine;

public class EnemyState : EntityState
{

    protected Enemy enemy;
    protected EnemySkillManager skillManager;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
        return true;
    }
    
    protected void FaceTarget()
    {
        if (enemy.player != null)
        {
            Vector3 directionToEnemy = enemy.player.position - enemy.transform.position;
            if (directionToEnemy.x > 0 && !enemy.facingRight)
                enemy.Flip();
            else if (directionToEnemy.x < 0 && enemy.facingRight)
                enemy.Flip();
        }
    }

}