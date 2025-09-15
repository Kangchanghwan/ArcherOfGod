using System;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable, ITargetable
{
    public static event Action OnEnemyDeath;
    private EntityHealth _health;

    private bool _facingRight;
    public Animator Animator { get; private set; }
    public Rigidbody2D Rigidbody2D { get; private set; }


    private void Awake()
    {
        Animator = GetComponentInChildren<Animator>();
        Rigidbody2D = GetComponent<Rigidbody2D>();

        _health = GetComponent<EntityHealth>();
    }


    private void Die()
    {
        OnEnemyDeath?.Invoke();
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
}