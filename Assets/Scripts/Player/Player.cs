using System;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    public Rigidbody2D rb { get; protected set; }
    public Animator animator { get; protected set; }

    public EntityHealth health;

    public bool facingRight = false;
    public bool canMove = true;

    public StateMachine stateMachine { get; protected set; }
    public static event Action OnPlayerDeath;

    [Header("Movement")] [SerializeField] public float moveSpeed = 5f;

    public Transform enemy;
    public ArrowManager arrowManager { get; private set; }
    private bool _manualRotation;

    public PlayerAttackState attackState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerCastingState castingState { get; private set; }
    public PlayerJumpShootState jumpShootState { get; private set; }
    public PlayerDeadState deadState { get; private set; }
    public PlayerIdleState idleState { get; private set; }

    public InputManager input { get; private set; }
    public PlayerSkillManager skillManager { get; private set; }

    public float xInput { get; private set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<EntityHealth>();
        animator = GetComponentInChildren<Animator>();

        stateMachine = new StateMachine();

        input = InputManagerSingleton.Instance.InputManager;
        skillManager = GetComponent<PlayerSkillManager>();
        arrowManager = GetComponent<ArrowManager>();

        enemy = GameObject.Find("Enemy").GetComponent<Transform>();

        attackState = new PlayerAttackState(stateMachine, "Attack", this);
        moveState = new PlayerMoveState(stateMachine, "Move", this);
        castingState = new PlayerCastingState(stateMachine, "Casting", this);
        jumpShootState = new PlayerJumpShootState(stateMachine, "JumpShoot", this);
        deadState = new PlayerDeadState(stateMachine, "Dead", this);
        idleState = new PlayerIdleState(stateMachine, "Idle", this);
    }

    private void Update()
    {
        stateMachine.currentState.Update();
    }

    private void OnEnable()
    {
        input.Enable();

        input.Player.Move.performed += ctx => xInput = ctx.ReadValue<float>();
        input.Player.Move.canceled += ctx => xInput = 0f;

        Enemy.OnEnemyDeath += HandlePlayerDeath;
    }

    private void Start()
    {
        stateMachine.Initialize(castingState);
    }

    private void OnDisable()
    {
        input.Disable();
        Enemy.OnEnemyDeath -= HandlePlayerDeath;
        CancelInvoke();
    }

    private void Die()
    {
        OnPlayerDeath?.Invoke();
        stateMachine.ChangeState(deadState);
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

    public void ActivateManualRotation(bool manualRotation) => _manualRotation = manualRotation;

    public bool ManualRotationActive() => _manualRotation;

    private void HandlePlayerDeath()
    {
        stateMachine.ChangeState(idleState);
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

    public void TakeDamage(float damage)
    {
        // 체력 감소
        health.ReduceHealth(damage);

        if (health.GetCurrentHealth() <= 0)
        {
            Die();
        }
        
    }
    
}