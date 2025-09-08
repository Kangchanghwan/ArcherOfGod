using System;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    [SerializeField]
    protected string animBoolName;

    protected Player Player;
    protected Animator Animator;
    protected Rigidbody2D Rigidbody2D;

    public bool TriggerCalled { get; private set; }



    protected virtual void Start()
    {
        Player = GetComponentInParent<Player>();
        Animator = Player?.Animator;
        Rigidbody2D = Player?.Rigidbody2D;
    }


    // public virtual void Update()
    // {
    //     // InputManager input = InputManagerSingleton.Instance.InputManager; 
    //     // if (input.Player.JumpShot.WasPressedThisFrame() && CanJumpShot())
    //     // {
    //     //     skillManager.jumpShoot.SetSillOnCooldown();
    //     //     Controller.ChangeState(Controller.SkillState);
    //     // }
    // }
        
    public virtual void Enter()
    {
        Animator.SetBool(animBoolName, true);
        TriggerCalled = false;
    }
    public virtual void Exit()
    {
        Animator.SetBool(animBoolName, false);
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
