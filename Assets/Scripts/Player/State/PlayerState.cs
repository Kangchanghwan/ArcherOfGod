using System;
using UnityEngine;

public abstract class PlayerState : MonoBehaviour
{
    protected Player Player;
    protected Animator Animator;
    protected Rigidbody2D Rigidbody2D;

    public bool TriggerCalled { get; private set; }

    protected virtual void Awake()
    {
        Player = GetComponentInParent<Player>(true);
    }

    private void EnsureInitialized()
    {
        if (Animator == null)
        {
            Animator = Player?.Animator;
        }

        if (Rigidbody2D == null)
        {
            Rigidbody2D = Player?.Rigidbody2D;
        }
    }

    public virtual void StateUpdate()
    {
    }

    protected abstract string GetAnimationName();

    public virtual void Enter()
    {
        EnsureInitialized();
        Animator.SetBool(GetAnimationName(), true);
        TriggerCalled = false;
    }

    public virtual void Exit()
    {
        Animator.SetBool(GetAnimationName(), false);
    }

    public void AnimationTrigger() => TriggerCalled = true;
}