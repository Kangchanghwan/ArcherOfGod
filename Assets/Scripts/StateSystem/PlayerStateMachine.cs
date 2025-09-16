using System;
using UnityEngine;

public class PlayerStateMachine: StateMachineBase
{
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

    protected override void Update()
    {
        base.Update();
        HandleSkillInput();
    }

    private void HandleSkillInput()
    {
        if (InputManagerSingleton.Instance?.InputManager?.Controller == null) 
            return;

        var controller = InputManagerSingleton.Instance.InputManager.Controller;

        if (controller.Skill_1.WasPressedThisFrame() && _skillState.TryUseSkill(1))
            ChangeState(_skillState);
        else if (controller.Skill_2.WasPressedThisFrame() && _skillState.TryUseSkill(2))
            ChangeState(_skillState);
        else if (controller.Skill_3.WasPressedThisFrame() && _skillState.TryUseSkill(3))
            ChangeState(_skillState);
        else if (controller.Skill_4.WasPressedThisFrame() && _skillState.TryUseSkill(4))
            ChangeState(_skillState);
        else if (controller.Skill_5.WasPressedThisFrame() && _skillState.TryUseSkill(5))
            ChangeState(_skillState);
    }
    protected override void HandleStateTransitions()
    {
        GetStateTransitionAction()?.Invoke();
    }
    private Action GetStateTransitionAction()
    {
        return CurrentState switch
        {
            MoveState => HandleMoveStateTransition,
            CastingState => HandleCastingStateTransition,
            AttackState => HandleAttackStateTransition,
            SkillState => HandleSkillStateTransition,
            DeadState => () => { }, // 죽은 상태에서는 전환 없음
            IdleState => () => { }, // 대기 상태에서는 전환 없음
            _ => () => { }
        };
    }

    private void HandleMoveStateTransition()
    {
        if (_skillState.OnSkill == false && _moveState.OnMove)
            ChangeState(_moveState);
        else
            ChangeState(_castingState);
    }

    private void HandleCastingStateTransition()
    {
        if (_moveState.OnMove)
            ChangeState(_moveState);
        else if (CurrentState.TriggerCalled)
            ChangeState(_attackState);
    }

    private void HandleAttackStateTransition()
    {
        if (_moveState.OnMove)
            ChangeState(_moveState);
        else if (CurrentState.TriggerCalled)
            ChangeState(_castingState);
    }

    private void HandleSkillStateTransition()
    {
        if (CurrentState.TriggerCalled)
            ChangeState(_castingState);
    }
    
    private void SubscribeToEvents()
    {
        Player.OnPlayerDeath += OnPlayerDeath;
        Enemy.OnEnemyDeath += OnEnemyDeath;
    }

    private void UnsubscribeFromEvents()
    {
        Player.OnPlayerDeath -= OnPlayerDeath;
        Enemy.OnEnemyDeath -= OnEnemyDeath;
    }

    private void OnPlayerDeath() => ChangeState(_deadState);
    private void OnEnemyDeath() => ChangeState(_idleState);


}