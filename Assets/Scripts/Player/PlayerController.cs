using System;
using UnityEngine;

public class PlayerContext : IContextBase
{
    public Player Player { get; set; }
    public PlayerController PlayerController { get; set; }
    public Animator Animator { get; set; }
    public Rigidbody2D RigidBody2D { get; set; }
}


public class PlayerController : MonoBehaviour
{
    public PlayerState CurrentState { get; private set; }
    public PlayerAttackState AttackState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerCastingState CastingState { get; private set; }
    public PlayerJumpShootState JumpShootState { get; private set; }
    public PlayerDeadState DeadState { get; private set; }
    public PlayerIdleState IdleState { get; private set; }

    [SerializeField]
    private ShotArrow shotArrow;
    
    private bool _manualRotation;
    
    private bool _facingRight;
    public bool CanMove { get; set; } = true;
    public bool OnMove { get; private set; }
    public float Input { get; private set; }
    
    private void Awake()
    {
        var context = new PlayerContext
        {
            Player = GetComponent<Player>(),
            PlayerController = this,
            Animator = GetComponentInChildren<Animator>(),
            RigidBody2D = GetComponent<Rigidbody2D>(),
        };

        AttackState = new PlayerAttackState(context, "Attack", shotArrow);
        MoveState = new PlayerMoveState(context, "Move");
        CastingState = new PlayerCastingState(context, "Casting");
        JumpShootState = new PlayerJumpShootState(context, "JumpShoot", 10f);
        DeadState = new PlayerDeadState(context, "Dead");
        IdleState = new PlayerIdleState(context, "Idle");
    }

    private void OnEnable()
    {
        InputManager input = InputManagerSingleton.Instance.InputManager;
        input.Enable();

        input.Player.Move.performed += ctx => Input = ctx.ReadValue<float>();
        input.Player.Move.canceled += _ => Input = 0f;
    }

    private void OnDisable()
    {
        InputManagerSingleton.Instance.InputManager.Disable();
    }

    private void Update()
    {
        OnMove = Math.Abs(Input) > 0.1f;
        CurrentState.Update();
    }

    public void Initialize(PlayerState startState)
    {
        CurrentState = startState;
        startState.Enter();
    }

    public void ChangeState(PlayerState newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }

    
    public void FlipController(float x = 1f)
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
    
    
    public void ActivateManualRotation(bool manualRotation) => _manualRotation = manualRotation;

    public bool ManualRotationActive() => _manualRotation;


    public void AnimationTrigger()
    {
        CurrentState.AnimationTrigger();
    }
}