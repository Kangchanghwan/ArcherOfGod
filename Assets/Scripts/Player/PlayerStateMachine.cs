using System;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    private PlayerState _currentState;

    private PlayerAttackState AttackState { get; set; }
    private PlayerMoveState MoveState { get; set; }
    private PlayerCastingState CastingState { get; set; }
    private PlayerDeadState DeadState { get; set; }
    private PlayerIdleState IdleState { get; set; }
    public PlayerSkillState SkillState { get; private set; }

    private void Awake()
    {
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
        if (InputManagerSingleton.Instance.InputManager.Controller.Skill_1.WasPressedThisFrame() &&
            SkillState.TryUseSkill(1))
        {
            ChangeState(SkillState);
            return;
        }
        if (InputManagerSingleton.Instance.InputManager.Controller.Skill_2.WasPressedThisFrame() &&
            SkillState.TryUseSkill(2))
        {
            ChangeState(SkillState);
            return;
        }
        if (InputManagerSingleton.Instance.InputManager.Controller.Skill_3.WasPressedThisFrame() &&
            SkillState.TryUseSkill(3))
        {
            ChangeState(SkillState);
            return;
        }
        if (InputManagerSingleton.Instance.InputManager.Controller.Skill_4.WasPressedThisFrame() &&
            SkillState.TryUseSkill(4))
        {
            ChangeState(SkillState);
            return;
        }
        if (InputManagerSingleton.Instance.InputManager.Controller.Skill_5.WasPressedThisFrame() &&
            SkillState.TryUseSkill(5))
        {
            ChangeState(SkillState);
            return;
        }
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