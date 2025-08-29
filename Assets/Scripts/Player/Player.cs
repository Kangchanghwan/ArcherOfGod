using UnityEngine;

public class Player : Entity
{
    
    [Header("Movement")]
    [SerializeField] public float moveSpeed = 5f;

    public Transform enemy;
    public ArrowManager arrowManager { get; private set; }
    private bool _manualRotation;

    public PlayerAttackState attackState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerCastingState castingState { get; private set; }
    public PlayerJumpShootState jumpShootState { get; private set; }

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
    }

    private void OnEnable()
    {
        input.Enable();

        input.Player.Move.performed += ctx => xInput = ctx.ReadValue<float>();
        input.Player.Move.canceled += ctx => xInput = 0f;
    }

    private void Start()
    {
        stateMachine.Initialize(castingState);
    }

    private void OnDisable()
    {
        input.Disable();
        CancelInvoke();
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