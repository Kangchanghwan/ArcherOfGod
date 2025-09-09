using UnityEngine;

public class EnemySkillState : EnemyState
{
    private SkillBase _skill;
    
    public override void Enter()
    {
        _skill = Enemy.GetRandomSkill();
        this.animBoolName =  _skill.AnimationName;
        base.Enter();
        Rigidbody2D.linearVelocity = Vector2.zero;
        _skill.Initialize(
            Rigidbody2D, 
            GameManager.Instance.EnemyOfTarget.GetTransform()
        );
        _skill.SetSkillOnCooldown();
        Enemy.FlipController();
        StartCoroutine(_skill.SkillCoroutine());
    }
    
    public override void Exit()
    {
        base.Exit();
    }
}
