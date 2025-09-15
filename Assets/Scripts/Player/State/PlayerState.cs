using System;
using UnityEngine;

public abstract class PlayerState : MonoBehaviour
{
    private bool _facingRight;

    protected Animator Animator;
    protected Rigidbody2D Rigidbody2D;
    protected PlayerStateMachine StateMachine;

    public bool TriggerCalled { get; private set; }

    protected virtual void Start()
    {
        StateMachine = GetComponentInParent<PlayerStateMachine>();

        Animator = StateMachine.Animator;
        Rigidbody2D = StateMachine.Rigidbody2D;
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
    
    private void Flip()
    {
        _facingRight = !_facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void FlipController(float x = 1f)
    {
        if (x > 0 && !_facingRight)
        {
            Flip();
        }
        else if (x < 0 && _facingRight)
        {
            Flip();
        }
    }

}