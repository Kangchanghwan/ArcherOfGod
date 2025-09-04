using System;
using System.Linq;
using UnityEngine;

public class Enemy: MonoBehaviour, IDamageable, ITargetable
{
    
    public Rigidbody2D rb { get; protected set; }
    public Animator animator { get; protected set; }

    public EntityHealth health;
    
    public bool facingRight = false;
    public bool canMove = true;
    
    public StateMachine stateMachine { get; protected set; }
    public static event Action OnEnemyDeath;

    [Header("Enemy Settings")]
    public ITargetable target;
    public float moveSpeed;
    public float moveStateTime;
    
    private bool _manualRotation;

    public ArrowManager arrowManager { get; private set; }
    public EnemySkillManager skillManager { get; private set; }

    public EnemyAttackState attackState { get; private set; }
    public EnemyMoveState moveState { get; private set; }
    public EnemyCastingState castingState { get; private set; }
    public EnemyJumpShootState jumpShootState { get; private set; }
    public EnemyIdleState idleState { get; private set; }
    public EnemyDeadState deadState { get; private set; }


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<EntityHealth>();
        animator = GetComponentInChildren<Animator>();
        
        stateMachine = new StateMachine();
        arrowManager = GetComponent<ArrowManager>();
        skillManager = GetComponent<EnemySkillManager>();
        
        castingState = new EnemyCastingState(stateMachine, "Casting", this);
        attackState = new EnemyAttackState(stateMachine, "Attack", this);
        moveState = new EnemyMoveState(stateMachine, "Move", this);
        jumpShootState = new EnemyJumpShootState(stateMachine, "JumpShoot", this);
        idleState = new EnemyIdleState(stateMachine, "Idle", this);
        deadState = new EnemyDeadState(stateMachine, "Dead", this);
    }

    private void Start()
    {
        stateMachine.Initialize(castingState);

        target = GameManager.Instance.EnemyOfTarget;
    }
    private void Update()
    {
        stateMachine.currentState.Update();
    }
    

    private void FlipController(float x)
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
    
    public void BezierShoot()
    {
        if (stateMachine.currentState == attackState ||
            stateMachine.currentState == jumpShootState)
        {
            arrowManager.BezierShoot(facingRight);
        }
    }

    public void LinearShoot()
    {
        if (stateMachine.currentState == attackState ||
            stateMachine.currentState == jumpShootState)
        {
            arrowManager.LinearShoot();
        }
    }
    public void SetVelocity(float xVelocity, float yVelocity)
    {
        rb.linearVelocity = new Vector2(xVelocity, yVelocity);
        FlipController(xVelocity);
    }

    public void Die()
    {
        OnEnemyDeath.Invoke();
        stateMachine.ChangeState(deadState);
    }

    public virtual void AnimationTrigger()
    {
        stateMachine.currentState.AnimationTrigger();
    }
    
    public void ActivateManualRotation(bool manualRotation) => _manualRotation = manualRotation;

    public bool ManualRotationActive() => _manualRotation;

    private void HandlePlayerDeath()
    {
        stateMachine.ChangeState(idleState);
    }

    private void OnEnable()
    {
        Player.OnPlayerDeath += HandlePlayerDeath;
    }

    private void OnDisable()
    {
        Player.OnPlayerDeath -= HandlePlayerDeath;
    }

    public void TakeDamage(float damage)
    {
        // 체력 감소
        health.ReduceHealth(damage);

        if (health.GetCurrentHealth() <= 0)
        {
            Die();
        }
        
    }

    public Transform GetTransform() => transform;
}
