using System;
using UnityEngine;

public abstract class PlayerState : MonoBehaviour
{
    protected Player Player;

    protected Animator Animator;
    protected Rigidbody2D Rigidbody2D;

    public bool TriggerCalled { get; private set; }

    protected virtual void Start()
    {
        Player = GetComponentInParent<Player>();

        Animator = Player.Animator;
        Rigidbody2D = Player.Rigidbody2D;
    }

    public virtual void StateUpdate()
    {
    }
    public virtual void Enter()
    {
        if (Animator == null) return;
        Animator.SetBool(GetAnimationName(), true);
        TriggerCalled = false;
    }
    protected abstract string GetAnimationName();
    
    public virtual void Exit()
    {
        Animator.SetBool(GetAnimationName(), false);
    }

    public void AnimationTrigger() => TriggerCalled = true;
    


}