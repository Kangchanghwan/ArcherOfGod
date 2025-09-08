using UnityEngine;

public class PlayerSkillState : PlayerState
{
    public SkillBase Skill { get; set; }


    public override void Enter()
    {
        if(Skill == null) return;
        animBoolName = Skill.AnimationName;
        base.Enter();
        
        Rigidbody2D.linearVelocity = Vector2.zero;
        Skill.Initialize(
            Rigidbody2D, 
            GameManager.Instance.PlayerOfTarget.GetTransform()
            );
        Skill.SetSkillOnCooldown();
        
        Player.CanMove = false;
        Player.CanSkill = false;
        Player.FlipController();

        StartCoroutine(Skill.SkillCoroutine());
    }

    
    public override void Exit()
    {
        base.Exit();
        Player.CanMove = true;
        Player.CanSkill = true;
    }
}