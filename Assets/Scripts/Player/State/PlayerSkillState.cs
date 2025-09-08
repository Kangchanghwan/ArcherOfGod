using UnityEditor.AnimatedValues;
using UnityEngine;

public class PlayerSkillState : PlayerState
{

    public SkillBase Skill { get; set; }
    
    public override void Enter()
    {
        base.Enter();
        animBoolName = Skill.AnimationName;
        Player.CanMove = false;
        Player.CanSkill = false;
    }
    
    public void Update()
    {
        Skill?.ExecuteSkill();
    }
    
    // protected void FaceTarget()
    // {
    //     if (player.target != null)
    //     {
    //         Vector3 directionToEnemy = player.target.GetTransform().position - player.transform.position;
    //         if (directionToEnemy.x > 0 && !playerController.facingRight)
    //             playerController.Flip();
    //     }
    // }
    
    public override void Exit()
    {
        base.Exit();
        Player.CanMove = true;
        Player.CanSkill = true;
    }
}