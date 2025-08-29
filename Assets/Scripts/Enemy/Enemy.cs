using UnityEngine;

public class Enemy : Entity
{
    [Header("Enemy Settings")]
    public Transform player;

    private bool _manualRotation;
    
    public ArrowManager arrowManager { get; private set; }
    public EnemySkillManager  skillManager { get; private set; }

    public EnemyAttackState attackState { get; private set; }
    public EnemyMoveState moveState { get; private set; }
    public EnemyCastingState castingState { get; private set; }
    public EnemyJumpShootState jumpShootState { get; private set; }

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
        
    }

    private void Start()
    {
        stateMachine.Initialize(castingState);
    }
    
    protected override void FlipController(float x)
    {
        if (x > 0 && facingRight)
        {
            Flip();
        }
        else if (x < 0 && !facingRight)
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

    public void ActivateManualRotation(bool manualRotation) => _manualRotation = manualRotation;

    public bool ManualRotationActive() => _manualRotation;
}