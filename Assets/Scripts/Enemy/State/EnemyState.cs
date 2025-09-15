using UnityEngine;

public abstract class EnemyState : MonoBehaviour
{
    protected Animator Animator;
    protected Rigidbody2D Rigidbody2D;
    
    private bool _facingRight;
    
    public bool TriggerCalled { get; private set; }

    protected virtual void Start()
    {
        EnemyStateMachine stateMachine = 
            GetComponentInParent<EnemyStateMachine>();
        
        Animator = stateMachine.Animator;
        Rigidbody2D = stateMachine.Rigidbody2D;
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
    
    public void FlipController(float x = -1f)
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
    
    private void Flip()
    {
        _facingRight = !_facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}