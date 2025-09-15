using UnityEngine;

public abstract class EnemyState : MonoBehaviour
{
    protected Enemy Enemy;

    protected Animator Animator;
    protected Rigidbody2D Rigidbody2D;

    public bool TriggerCalled { get; private set; }

    protected virtual void Start()
    {
        Enemy = GetComponentInParent<Enemy>();

        Animator = Enemy.Animator;
        Rigidbody2D = Enemy.Rigidbody2D;
    }

    protected abstract string GetAnimationName();


    public virtual void Enter()
    {
        if (Animator == null) return;
        Animator.SetBool(GetAnimationName(), true);
        TriggerCalled = false;
        Debug.Log("Entered " + this.GetType().Name);
    }

    public virtual void Exit()
    {
        Animator.SetBool(GetAnimationName(), false);
        Debug.Log("Exited " + this.GetType().Name);
    }

    public void AnimationTrigger() => TriggerCalled = true;


    public virtual void StateUpdate()
    {
    }


}