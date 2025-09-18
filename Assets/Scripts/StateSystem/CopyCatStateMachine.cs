using System;
using UnityEngine;

public class CopyCatStateMachine : StateMachineBase
{
    // 통합된 상태들
    private AttackState _attackState;
    private MoveState _moveState;
    private CastingState _castingState;
    private SkillState _skillState;
    private FadeInState _fadeInState;
    private FadeOutState _fadeOutState;

    private void Awake()
    {
        _attackState = GetComponentInChildren<AttackState>();
        _moveState = GetComponentInChildren<MoveState>();
        _castingState = GetComponentInChildren<CastingState>();
        _skillState = GetComponentInChildren<SkillState>();
        _fadeInState = GetComponentInChildren<FadeInState>();
        _fadeOutState = GetComponentInChildren<FadeOutState>();
    }


    private void OnEnable()
    {
        SubscribeToEvents();
        Initialize(_fadeInState);
    }

    private void OnDisable()
    {
        UnsubscribeFromEvents();
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
            FadeInState => HandleFadeInStateTransition,
            FadeOutState => () => { },
            DeadState => () => { }, // 죽은 상태에서는 전환 없음
            IdleState => () => { }, // 대기 상태에서는 전환 없음
            _ => () => { }
        };
    }

    private void HandleFadeInStateTransition()
    {
        if(_fadeInState.isDone)
            ChangeState(_castingState);
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
    }

    private void UnsubscribeFromEvents()
    {
        Enemy.OnEnemyDeath -= OnEnemyDeath;
    }

    private void OnEnemyDeath() => ChangeState(_fadeOutState);
    public void OnChangeFadeOutState() => ChangeState(_fadeOutState);
}