using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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
    [SerializeField] private ShotArrow shotArrow;

    public PlayerAttackState AttackState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerCastingState CastingState { get; private set; }
    public PlayerDeadState DeadState { get; private set; }
    public PlayerIdleState IdleState { get; private set; }

    private PlayerState _currentState;

    private bool _facingRight;
    private bool _onMove;
    public bool CanMove { get; set; } = true;
    public float InputX { get; private set; }

    [SerializeField] private SkillBase[] SkillBases;
    public bool CanSkill { get; set; } = true;

    private void Awake()
    {
        var context = CreateContext();

        AttackState = new PlayerAttackState(context, "Attack", shotArrow);
        MoveState = new PlayerMoveState(context, "Move");
        CastingState = new PlayerCastingState(context, "Casting");
        DeadState = new PlayerDeadState(context, "Dead");
        IdleState = new PlayerIdleState(context, "Idle");

        SkillBases = GetComponentsInChildren<SkillBase>(true);
    }

    private PlayerContext CreateContext()
    {
        var context = new PlayerContext
        {
            Player = GetComponent<Player>(),
            PlayerController = this,
            Animator = GetComponentInChildren<Animator>(),
            RigidBody2D = GetComponent<Rigidbody2D>(),
        };
        return context;
    }

    private void OnEnable()
    {
        InputManager input = InputManagerSingleton.Instance.InputManager;
        input.Enable();

        input.Player.Move.performed += ctx => InputX = ctx.ReadValue<float>();
        input.Player.Move.canceled += _ => InputX = 0f;

        input.Player.Skill_1.performed += _ => TryUseSkill(0);
        input.Player.Skill_2.performed += _ => TryUseSkill(1);
        input.Player.Skill_3.performed += _ => TryUseSkill(2);
    }

    private void OnDisable()
    {
        InputManagerSingleton.Instance.InputManager.Disable();
    }

    private void Update()
    {
        StateChangeAction()?.Invoke();
        _currentState.Update();
    }

    private Action StateChangeAction()
    {
        _onMove = Math.Abs(InputX) > 0.1f;
        
        return _currentState switch
        {
            PlayerCastingState => () =>
            {
                if (_onMove)
                    ChangeState(MoveState);
                else if (_currentState.TriggerCalled)
                    ChangeState(AttackState);
            },

            PlayerAttackState => () =>
            {
                if (_onMove)
                    ChangeState(MoveState);
                else if (_currentState.TriggerCalled)
                    ChangeState(CastingState);
            },

            PlayerMoveState => () =>
            {
                if (CanMove && _onMove)
                    ChangeState(MoveState);
                else
                    ChangeState(CastingState);
            },

            PlayerSkillState => () =>
            {
                if (_currentState.TriggerCalled)
                    ChangeState(CastingState);
            },

            PlayerDeadState => () => { }, // 상태 변경 없음
            PlayerIdleState => () => { }, // 상태 변경 없음

            _ => () => { } // 기본값
        };
    }


    public void Initialize(PlayerState startState)
    {
        _currentState = startState;
        startState.Enter();
    }

    public void ChangeState(PlayerState newState)
    {
        _currentState.Exit();
        _currentState = newState;
        _currentState.Enter();
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

    public void AnimationTrigger()
    {
        _currentState.AnimationTrigger();
    }

    private void TryUseSkill(int skillNumber)
    {
        var skill = SkillBases[skillNumber];

        if (skill == null) return;
        if (skill.CanUseSkill() == false) return;
        if (CanSkill == false) return;

        var skillState = new PlayerSkillState(CreateContext(), skill);

        ChangeState(skillState);
    }
}