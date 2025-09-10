using System;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable, ITargetable
{
    public static event Action OnEnemyDeath;

    public Rigidbody2D Rigidbody2D { get; private set; }
    public Animator Animator { get; private set; }

    private EntityHealth _health;
    private bool _facingRight;
    public bool CanSkill => SkillJumpShoot.CanUseSkill();
    public bool CanMove { get; set; } = true;

    public EnemyAttackState AttackState { get; private set; }
    public EnemyMoveState MoveState { get; private set; }
    public EnemyCastingState CastingState { get; private set; }
    public EnemySkillState SkillState { get; private set; }
    public EnemyIdleState IdleState { get; private set; }
    public EnemyDeadState DeadState { get; private set; }

    public EnemyState CurrentState { get; set; }

    public SkillJumpShoot SkillJumpShoot { get; private set; }


    private void Awake()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Animator = GetComponentInChildren<Animator>();

        CastingState = GetComponentInChildren<EnemyCastingState>();
        AttackState = GetComponentInChildren<EnemyAttackState>();
        MoveState = GetComponentInChildren<EnemyMoveState>();
        SkillState = GetComponentInChildren<EnemySkillState>();
        IdleState = GetComponentInChildren<EnemyIdleState>();
        DeadState = GetComponentInChildren<EnemyDeadState>();

        SkillJumpShoot = GetComponentInChildren<SkillJumpShoot>();

        _health = GetComponent<EntityHealth>();
    }

    private void Update()
    {
        CurrentState.StateUpdate();
    }


    public void FlipController(float x = -1f)
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

    private void Die()
    {
        OnEnemyDeath?.Invoke();
    }

    public virtual void AnimationTrigger()
    {
        CurrentState.AnimationTrigger();
    }


    public void TakeDamage(float damage)
    {
        // 체력 감소
        _health.ReduceHealth(damage);

        if (_health.GetCurrentHealth() <= 0)
        {
            Die();
        }
    }

    public SkillBase GetRandomSkill()
    {
        return SkillJumpShoot;
    }

    public Transform GetTransform() => transform;
}