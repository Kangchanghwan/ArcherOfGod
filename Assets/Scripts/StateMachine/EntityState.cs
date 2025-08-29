using UnityEngine;

public abstract class EntityState
{
    protected StateMachine stateMachine;
    protected string animBoolName;

    protected Animator animator;
    protected Rigidbody2D rigidbody2D;

    protected bool triggerCalled;
    protected float stateTimer;

    protected EntityState(StateMachine stateMachine, string animBoolName)
    {
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        animator.SetBool(animBoolName, true);
        triggerCalled = false;
    }

    public virtual void Update()
    {
        stateTimer += Time.deltaTime;
    }
    public virtual void Exit()
    {
        animator.SetBool(animBoolName, false);
    }


    public void AnimationTrigger() => triggerCalled = true;
}
