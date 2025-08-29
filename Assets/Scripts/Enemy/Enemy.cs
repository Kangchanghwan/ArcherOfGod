using System;
using UnityEngine;

public class Enemy : Entity
{
    public static event Action OnEnemyDeath;

    [Header("Enemy Settings")]
    public Transform player;
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


    protected override void Awake()
    {
        base.Awake(); // Entity의 Awake 호출

        player = GameObject.Find("Player").GetComponent<Transform>();

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
    }

    protected override void FlipController(float x)
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

    public override void EntityDeath()
    {
        base.EntityDeath();
        OnEnemyDeath.Invoke();
        stateMachine.ChangeState(deadState);
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
}
