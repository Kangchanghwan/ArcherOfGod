using UnityEngine;

public abstract class EnemyState : MonoBehaviour
{
    protected Enemy Enemy;
    protected Animator Animator;
    protected Rigidbody2D Rigidbody2D;

    public bool TriggerCalled { get; private set; }

    protected virtual void Awake()
    {
        Enemy = GetComponentInParent<Enemy>(true);
    }

    protected abstract string GetAnimationName();

    private void EnsureInitialized()
    {
        if (Animator == null)
        {
            Animator = Enemy?.Animator;
        }

        if (Rigidbody2D == null)
        {
            Rigidbody2D = Enemy?.Rigidbody2D;
        }
    }

    public virtual void Enter()
    {
        EnsureInitialized();
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