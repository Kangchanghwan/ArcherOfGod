using UnityEngine;

public class PlayerState : EntityState
{

    protected Player player;
    protected InputManager input;
    protected PlayerSkillManager skillManager;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public PlayerState(StateMachine stateMachine, string animBoolName, Player player) : 
        base(stateMachine, animBoolName)
    {
        this.player = player;
        animator = player.animator;
        rigidbody2D = player.rb;
        input = player.input;
        skillManager = player.skillManager;
    }
    
    public override void Update()
    {
        base.Update();
        if (input.Player.JumpShot.WasPressedThisFrame() && CanJumpShot())
        {
            skillManager.jumpShoot.SetSillOnCooldown();
            stateMachine.ChangeState(player.jumpShootState);
        }
    }
    
    private bool CanJumpShot()
    {
        if (skillManager.jumpShoot.CanUseSkill() == false)
        {
            return false;
        }
        if (player.stateMachine.currentState == player.jumpShootState)
        {
            return false;
        }
        return true;
    }
    
    protected void FaceTarget()
    {
        if (player.target != null)
        {
            Vector3 directionToEnemy = player.target.GetTransform().position - player.transform.position;
            if (directionToEnemy.x > 0 && !player.facingRight)
                player.Flip();
            else if (directionToEnemy.x < 0 && player.facingRight)
                player.Flip();
        }
    }

}
