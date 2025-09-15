using System;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    public Animator Animator {get; private set;}
    public Rigidbody2D Rigidbody2D { get; private set; }

    private EnemyAttackState AttackState { get; set; }
    private EnemyMoveState MoveState { get; set; }
    private EnemyCastingState CastingState { get; set; }
    private EnemySkillState SkillState { get; set; }
    private EnemyIdleState IdleState { get; set; }
    private EnemyDeadState DeadState { get; set; }

    private EnemyState CurrentState { get; set; }
    public SkillJumpShoot SkillJumpShoot { get; private set; }

    private void Awake()
    {
        Animator = GetComponentInChildren<Animator>();
        Rigidbody2D = GetComponent<Rigidbody2D>();
        
        CastingState = GetComponentInChildren<EnemyCastingState>();
        AttackState = GetComponentInChildren<EnemyAttackState>();
        MoveState = GetComponentInChildren<EnemyMoveState>();
        SkillState = GetComponentInChildren<EnemySkillState>();
        IdleState = GetComponentInChildren<EnemyIdleState>();
        DeadState = GetComponentInChildren<EnemyDeadState>();

        SkillJumpShoot = GetComponentInChildren<SkillJumpShoot>();
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
        Initialize(CastingState);
    }
   
    private void Update()
    {
        StateChangeAction()?.Invoke();
        CurrentState.StateUpdate();
    }

    private Action StateChangeAction()
    {
        return CurrentState switch
        {
            EnemyAttackState => () =>
            {
                if (CurrentState.TriggerCalled)
                {
                    if (UnityEngine.Random.Range(0, 2) == 1)
                        ChangeState(MoveState);
                    else
                        ChangeState(CastingState);
                }
            },
            EnemyCastingState => () =>
            {
                if (CurrentState.TriggerCalled)
                {
                    if (SkillState.CanSkill)
                        ChangeState(SkillState);
                    else
                        ChangeState(AttackState);
                }
            },
            EnemySkillState => () =>
            {
                if (CurrentState.TriggerCalled)
                    ChangeState(MoveState);
            },
            EnemyMoveState => () =>
            {
                if (MoveState.OnMove == false)
                    ChangeState(CastingState);
            },
            EnemyDeadState => () => { },
            EnemyIdleState => () => { },
            _ => () => { } // 기본값
        };
    }

    private Action ChangeStateIdle() => () => ChangeState(IdleState);
    private Action ChangeStateDead() => () => ChangeState(DeadState);

    private void Initialize(EnemyState startState)
    {
        CurrentState = startState;
        CurrentState.Enter();
    }

    private void ChangeState(EnemyState newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }
    public virtual void AnimationTrigger()
    {
        CurrentState.AnimationTrigger();
    }


}