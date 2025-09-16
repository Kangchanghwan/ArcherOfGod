using UnityEngine;

public abstract class StateBase: MonoBehaviour
{
    protected EntityBase Entity;

    protected Animator Animator;
    protected Rigidbody2D Rigidbody2D;

    public bool TriggerCalled { get; private set; }

    protected virtual void Start()
    {
        Entity = GetComponentInParent<EntityBase>();

        Animator = Entity.Animator;
        Rigidbody2D = Entity.Rigidbody2D;
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
    
    public virtual void Exit()
    {
        Animator.SetBool(GetAnimationName(), false);
    }

    public void AnimationTrigger() => TriggerCalled = true;
    
    protected abstract string GetAnimationName();
}