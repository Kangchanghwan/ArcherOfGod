using UnityEngine;

public class PlayerState
{
    private readonly string _animBoolName;

    protected Player Player;
    protected readonly Animator Animator;
    protected readonly Rigidbody2D Rigidbody2D;

    public bool TriggerCalled { get; private set; }



    public PlayerState(PlayerContext context, string animBoolName)
    {
        this.Player = context.Player;
        this.Rigidbody2D =  context.RigidBody2D;
        this.Animator = context.Animator;
        this._animBoolName = animBoolName;
    }
    
    public virtual void Update()
    {
        // InputManager input = InputManagerSingleton.Instance.InputManager; 
        // if (input.Player.JumpShot.WasPressedThisFrame() && CanJumpShot())
        // {
        //     skillManager.jumpShoot.SetSillOnCooldown();
        //     Controller.ChangeState(Controller.SkillState);
        // }
    }
        
    public virtual void Enter()
    {
        Animator.SetBool(_animBoolName, true);
        TriggerCalled = false;
    }
    public virtual void Exit()
    {
        Animator.SetBool(_animBoolName, false);
    }
    // private bool CanJumpShot()
    // {
    //     if (skillManager.jumpShoot.CanUseSkill() == false)
    //     {
    //         return false;
    //     }
    //     if (Controller.CurrentState ==Controller.SkillState)
    //     {
    //         return false;
    //     }
    //     return true;
    // }

    public void AnimationTrigger() => TriggerCalled = true;
}
