using System;
using Component.Entity;
using UnityEngine;

public class BotStateMachine : StateMachineBase
{
    // 통합된 상태들
    private AttackState _attackState;
    private MoveState _moveState;
    private CastingState _castingState;
    private DeadState _deadState;
    private IdleState _idleState;
    private SkillState _skillState;

    private void Awake()
    {
        _attackState = GetComponentInChildren<AttackState>();
        _moveState = GetComponentInChildren<MoveState>();
        _castingState = GetComponentInChildren<CastingState>();
        _deadState = GetComponentInChildren<DeadState>();
        _idleState = GetComponentInChildren<IdleState>();
        _skillState = GetComponentInChildren<SkillState>();
    }


    private void OnEnable()
    {
        SubscribeToEvents();
    }

    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }

    private void Start()
    {
        Initialize(_castingState);
    }

    protected override void HandleStateTransitions()
    {
        GetStateTransitionAction()?.Invoke();
    }
    
    
    private Action GetStateTransitionAction()
    {
        return CurrentState switch
        {
            AttackState => HandleAttackStateTransition,
            CastingState => HandleCastingStateTransition,
            SkillState => HandleSkillStateTransition,
            MoveState => HandleMoveStateTransition,
            DeadState => () => { }, // 죽은 상태에서는 전환 없음
            IdleState => () => { }, // 대기 상태에서는 전환 없음
            _ => () => { }
        };
    }

    private void HandleAttackStateTransition()
    {
        if (CurrentState.TriggerCalled)
        {
            if (UnityEngine.Random.Range(0f, 1f) < 0.5f)
                ChangeState(_moveState);
            else
                ChangeState(_castingState);
        }
    }

    private void HandleCastingStateTransition()
    {
        if (CurrentState.TriggerCalled)
        {
            if (_skillState.TryUseRandomSkill())
                ChangeState(_skillState);
            else
                ChangeState(_attackState);
        }
    }

    private void HandleSkillStateTransition()
    {
        if (CurrentState.TriggerCalled)
            ChangeState(_moveState);
    }

    private void HandleMoveStateTransition()
    {
        if (!_moveState.OnMove)
            ChangeState(_castingState);
    }

    private void SubscribeToEvents()
    {
        Enemy.OnEnemyDeath += OnEnemyDeath;
        // PlayerController.OnPlayerDeath += OnPlayerDeath;
    }

    private void UnsubscribeFromEvents()
    {
        Enemy.OnEnemyDeath -= OnEnemyDeath;
        // PlayerController.OnPlayerDeath -= OnPlayerDeath;
    }

    private void OnEnemyDeath() => ChangeState(_deadState);
    private void OnPlayerDeath() => ChangeState(_idleState);
    
}