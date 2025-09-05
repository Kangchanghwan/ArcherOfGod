using UnityEngine;

public class PlayerSkillState : PlayerState
{

    private readonly SkillBase _skill;


    public PlayerSkillState(PlayerContext context, SkillBase skill) : base(context,  skill.AnimationName)
    {
        _skill = skill;
        _skill.Initialize(context);
    }

    public override void Enter()
    {
        base.Enter();
        Controller.CanMove = false;
        Controller.CanSkill = false;
    }
    
    public override void Update()
    {
        base.Update();
        
        _skill.ExecuteSkill();
    
        if (TriggerCalled)
        {
            Controller.ChangeState(Controller.CastingState);
        }
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
        Controller.CanMove = true;
        Controller.CanSkill = true;
    }
}