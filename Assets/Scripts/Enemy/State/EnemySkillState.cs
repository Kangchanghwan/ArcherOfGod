using UnityEngine;

public class EnemySkillState : EnemyState
{
    [Header("Jump Settings")]
    public float jumpForce = 10f;
    
    
    public override void Enter()
    {
        base.Enter();
        Enemy.CanMove = false;
    }
    
    public override void Exit()
    {
        base.Exit();
        Enemy.CanMove = true;
    }
}
