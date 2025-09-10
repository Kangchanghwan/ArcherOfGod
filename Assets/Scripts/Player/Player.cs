using System;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable, ITargetable
{
    public static event Action OnPlayerDeath;

    public bool CanSkill { get; set; } = true;
    public bool CanMove { get; set; } = true;
    public bool OnMove { get; set; }
    public PlayerState CurrentState { get; set; }
    
    private bool _facingRight;
    private EntityHealth _health;

    public Rigidbody2D Rigidbody2D { get; private set; }
    public Animator Animator { get; private set; }

    public PlayerAttackState AttackState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerCastingState CastingState { get; private set; }
    public PlayerDeadState DeadState { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerSkillState SkillState { get; private set; }



    private void Awake()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Animator = GetComponentInChildren<Animator>();

        AttackState = GetComponentInChildren<PlayerAttackState>();
        MoveState = GetComponentInChildren<PlayerMoveState>();
        CastingState = GetComponentInChildren<PlayerCastingState>();
        DeadState = GetComponentInChildren<PlayerDeadState>();
        IdleState = GetComponentInChildren<PlayerIdleState>();
        SkillState = GetComponentInChildren<PlayerSkillState>();

        _health = GetComponent<EntityHealth>();
    }

    private void Update()
    {
        CurrentState.StateUpdate();
    }

    private void Die()
    {
        OnPlayerDeath?.Invoke();
    }

    private void Flip()
    {
        _facingRight = !_facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void FlipController(float x = 1f)
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

    public void AnimationTrigger()
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

    public Transform GetTransform() => transform;
}