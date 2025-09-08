using System;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    private Player _player;


    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    private void Start()
    {
        Initialize(_player.CastingState);
    }

    private void Update()
    {
        StateChangeAction()?.Invoke();
    }

    private Action StateChangeAction()
    {
        return _player.CurrentState switch
        {
            PlayerMoveState => () =>
            {
                if (_player.CanMove && _player.OnMove)
                    ChangeState(_player.MoveState);
                else
                    ChangeState(_player.CastingState);
            },
            PlayerCastingState => () =>
            {
                if (_player.OnMove)
                    ChangeState(_player.MoveState);
                else if (_player.CurrentState.TriggerCalled)
                    ChangeState(_player.AttackState);
            },
            PlayerAttackState => () =>
            {
                if (_player.OnMove)
                    ChangeState(_player.MoveState);
                else if (_player.CurrentState.TriggerCalled)
                    ChangeState(_player.CastingState);
            },
            PlayerSkillState => () =>
            {
                if (_player.CurrentState.TriggerCalled)
                    ChangeState(_player.CastingState);
            },
            PlayerDeadState => () => { }, // 상태 변경 없음
            PlayerIdleState => () => { }, // 상태 변경 없음

            _ => () => { } // 기본값
        };
    }


    public void Initialize(PlayerState startState)
    {
        _player.CurrentState = startState;
        _player.CurrentState.Enter();
    }

    public void ChangeState(PlayerState newState)
    {
        _player.CurrentState.Exit();
        _player.CurrentState = newState;
        _player.CurrentState.Enter();
    }


    public void TryUseSkill(int skillNumber)
    {
        var skill = skillNumber switch
        {
            1 => _player.SkillJumpShoot,
            _ => null
        };

        if (skill == null) return;
        if (skill.CanUseSkill() == false) return;
        if (_player.CanSkill == false) return;

        _player.SkillState.Skill = skill;
        ChangeState(_player.SkillState);
    }

    private void DieState() => Player.OnPlayerDeath += () => ChangeState(_player.DeadState);
    private void TargetDieState() => Enemy.OnEnemyDeath += () => ChangeState(_player.IdleState);

    public void UpdateInputX(float inputX)
    {
        _player.OnMove = Mathf.Abs(inputX) > 0.1f;
        _player.MoveState.XInput = inputX;
    }
}