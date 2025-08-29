using UnityEngine;

public class Entity : MonoBehaviour
{
  
    public Rigidbody2D rb { get; protected set; }
    public Animator animator { get; protected set; }
    
    public bool facingRight = false;
    public bool canMove = true;
    
    public StateMachine stateMachine { get; protected set; }
    
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        
        stateMachine = new StateMachine();
    }
    
    protected virtual void Update()
    {
        stateMachine.currentState.Update();
    }
    
    public virtual void SetVelocity(float xVelocity, float yVelocity)
    {
        rb.linearVelocity = new Vector2(xVelocity, yVelocity);
        FlipController(xVelocity);
    }
    
    protected virtual void FlipController(float x)
    {
        if (x > 0 && !facingRight)
        {
            Flip();
        }
        else if (x < 0 && facingRight)
        {
            Flip();
        }
    }
    
    public virtual void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
    
    public virtual void AnimationTrigger()
    {
        stateMachine.currentState.AnimationTrigger();
    }
}