using System;
using UnityEngine;

public class Player : Entity
{
    public static event Action OnPlayerDeath;

    [Header("Movement")]
    [SerializeField] public float moveSpeed = 5f;

    public Transform enemy;
    public ArrowManager arrowManager { get; private set; }
    private bool _manualRotation;

    public PlayerAttackState attackState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerCastingState castingState { get; private set; }
    public PlayerJumpShootState jumpShootState { get; private set; }
    public PlayerDeadState deadState { get; private set; }
    public PlayerIdleState idleState { get; private set; }

    public PlayerInputManager input { get; private set; }
    public PlayerSkillManager skillManager { get; private set; }

    public float xInput { get; private set; }

    protected override void Awake()
    {
        base.Awake(); // Entity의 Awake 호출
        
        input = new PlayerInputManager();
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

    public override void EntityDeath()
    {
        base.EntityDeath();

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
 
}
