using System;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    private PlayerState _currentState;

    public Animator Animator { get; private set; }
    public Rigidbody2D Rigidbody2D { get; private set; }
    private PlayerAttackState AttackState { get; set; }
    private PlayerMoveState MoveState { get; set; }
    private PlayerCastingState CastingState { get; set; }
    private PlayerDeadState DeadState { get; set; }
    private PlayerIdleState IdleState { get; set; }
    public PlayerSkillState SkillState { get; private set; }

    private void Awake()
    {
        Animator = GetComponentInChildren<Animator>();
        Rigidbody2D = GetComponent<Rigidbody2D>();

        AttackState = GetComponentInChildren<PlayerAttackState>();
        MoveState = GetComponentInChildren<PlayerMoveState>();
        CastingState = GetComponentInChildren<PlayerCastingState>();
        DeadState = GetComponentInChildren<PlayerDeadState>();
        IdleState = GetComponentInChildren<PlayerIdleState>();
        SkillState = GetComponentInChildren<PlayerSkillState>();
    }

    private void OnEnable()
    {
        Player.OnPlayerDeath += ChangeStateDead();
        Enemy.OnEnemyDeath += ChangeStateIdle();
    }

    private void OnDisable()
    {
        Player.OnPlayerDeath -= ChangeStateDead();
        Enemy.OnEnemyDeath -= ChangeStateIdle();
    }

    private void Start()
    {
        Initialize(CastingState);
    }

    private void Update()
    {
        StateChangeAction()?.Invoke();
        OnSkillEventListener();
        _currentState.StateUpdate();
    }

    private void OnSkillEventListener()
    {
        if (SkillState.TriggerSkill)
            ChangeState(SkillState);
    }
    

    private Action StateChangeAction()
    {
        return _currentState switch
        {
            PlayerMoveState => () =>
            {
                if (SkillState.OnSkill == false && MoveState.OnMove())
                    ChangeState(MoveState);
                else
                    ChangeState(CastingState);
            },
            PlayerCastingState => () =>
            {
                if (MoveState.OnMove())
                    ChangeState(MoveState);
                else if (_currentState.TriggerCalled)
                    ChangeState(AttackState);
            },
            PlayerAttackState => () =>
            {
                if (MoveState.OnMove())
                    ChangeState(MoveState);
                else if (_currentState.TriggerCalled)
                    ChangeState(CastingState);
            },
            PlayerSkillState => () =>
            {
                if (_currentState.TriggerCalled)
                    ChangeState(CastingState);
            },
            PlayerDeadState => () => { },
            PlayerIdleState => () => { },

            _ => () => { } // 기본값
        };
    }


    private Action ChangeStateIdle() => () => ChangeState(IdleState);

    private Action ChangeStateDead() => () => ChangeState(DeadState);

    public void Initialize(PlayerState startState)
    {
        _currentState = startState;
        _currentState.Enter();
    }

    public void ChangeState(PlayerState newState)
    {
        _currentState.Exit();
        _currentState = newState;
        _currentState.Enter();
    }

    public void AnimationTrigger()
    {
        _currentState.AnimationTrigger();
    }
}