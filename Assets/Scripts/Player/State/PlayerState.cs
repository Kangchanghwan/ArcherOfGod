using UnityEngine;

public class PlayerState
{

    protected PlayerSkillManager skillManager;
    
    private readonly string _animBoolName;

    protected Player player;
    protected Animator animator;
    protected Rigidbody2D rigidbody2D;
    protected PlayerController playerController;

    protected bool triggerCalled;

    

    public PlayerState(PlayerContext context, string animBoolName)
    {
        this.player = context.Player;
        this.rigidbody2D =  context.RigidBody2D;
        this.playerController = context.PlayerController;
        this.animator = context.Animator;
        this._animBoolName = animBoolName;
    }
    
    public virtual void Update()
    {
        InputManager input = InputManagerSingleton.Instance.InputManager; 
        if (input.Player.JumpShot.WasPressedThisFrame() && CanJumpShot())
        {
            skillManager.jumpShoot.SetSillOnCooldown();
            playerController.ChangeState(playerController.JumpShootState);
        }
    }
        
    public virtual void Enter()
    {
        animator.SetBool(_animBoolName, true);
        triggerCalled = false;
    }
    public virtual void Exit()
    {
        animator.SetBool(_animBoolName, false);
    }
    private bool CanJumpShot()
    {
        if (skillManager.jumpShoot.CanUseSkill() == false)
        {
            return false;
        }
        if (playerController.CurrentState ==playerController.JumpShootState)
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
            if (directionToEnemy.x > 0 && !playerController.facingRight)
                playerController.Flip();
        }
    }
    
    public void AnimationTrigger() => triggerCalled = true;
}
