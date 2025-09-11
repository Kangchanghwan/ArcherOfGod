using System;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    private Player _player;


    private void Awake()
    {
        _player = GetComponent<Player>();
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
            PlayerDeadState => () => { },
            PlayerIdleState => () => { },

            _ => () => { } // 기본값
        };
    }


    private Action ChangeStateIdle() => () => ChangeState(_player.IdleState);

    private Action ChangeStateDead() => () => ChangeState(_player.DeadState);

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
        SkillBase skill = skillNumber switch
        {
            0 => _player.SkillState.SkillJumpShoot,
            1 => _player.SkillState.SkillBombShoot,
            2 => _player.SkillState.SkillRipedFire,
            _ => null
        };

        if (skill == null) return;
        if (skill.CanUseSkill() == false) return;
        if (_player.CanSkill == false) return;

        _player.SkillState.CurrentSkill = skill;
        ChangeState(_player.SkillState);
    }

    public void UpdateInputX(float inputX)
    {
        _player.OnMove = Mathf.Abs(inputX) > 0.1f;
        _player.MoveState.XInput = inputX;
    }
}