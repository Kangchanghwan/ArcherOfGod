using System;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    private Enemy _enemy;

    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
    }

    private void OnEnable()
    {
        Enemy.OnEnemyDeath += ChangeStateDead();
        Player.OnPlayerDeath += ChangeStateIdle();
    }

    private void OnDisable()
    {
        Enemy.OnEnemyDeath -= ChangeStateDead();
        Player.OnPlayerDeath -= ChangeStateIdle();
    }


    private void Start()
    {
        Initialize(_enemy.CastingState);
    }

    private void Update()
    {
        StateChangeAction()?.Invoke();
    }

    private Action StateChangeAction()
    {
        return _enemy.CurrentState switch
        {
            EnemyAttackState => () =>
            {
                if (_enemy.CurrentState.TriggerCalled)
                    ChangeState(_enemy.MoveState);
            },
            EnemyCastingState => () =>
            {
                if (_enemy.CurrentState.TriggerCalled)
                {
                    if (_enemy.CanSkill)
                        ChangeState(_enemy.SkillState);
                    else
                        ChangeState(_enemy.AttackState);
                }
            },
            EnemySkillState => () =>
            {
                if (_enemy.CurrentState.TriggerCalled)
                    ChangeState(_enemy.MoveState);
            },
            EnemyMoveState => () =>
            {
                if (_enemy.CanMove == false)
                    ChangeState(_enemy.CastingState);
            },
            EnemyDeadState => () => { },
            EnemyIdleState => () => { },
            _ => () => { } // 기본값
        };
    }

    private Action ChangeStateIdle() => () => ChangeState(_enemy.IdleState);

    private Action ChangeStateDead() => () => ChangeState(_enemy.DeadState);

    private void Initialize(EnemyState startState)
    {
        _enemy.CurrentState = startState;
        _enemy.CurrentState.Enter();
    }

    private void ChangeState(EnemyState newState)
    {
        _enemy.CurrentState.Exit();
        _enemy.CurrentState = newState;
        _enemy.CurrentState.Enter();
    }
}