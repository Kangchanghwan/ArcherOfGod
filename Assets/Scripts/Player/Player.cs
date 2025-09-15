using System;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable, ITargetable
{
    public static event Action OnPlayerDeath;

    public Animator Animator { get; private set; }
    public Rigidbody2D Rigidbody2D { get; private set; }
    
    private EntityHealth _health;
    private bool _facingRight;

    private void Awake()
    {
        Animator = GetComponentInChildren<Animator>();
        Rigidbody2D = GetComponent<Rigidbody2D>();
        _health = GetComponent<EntityHealth>();
    }

    private void OnEnable()
    {
        InputManagerSingleton.Instance.Enable();
        OnPlayerDeath += HandlePlayerDeath;
    }
    private void OnDisable()
    {
        OnPlayerDeath -= HandlePlayerDeath;
    }
    
    private void HandlePlayerDeath()  =>  InputManagerSingleton.Instance.Disable();
    private void Die() => OnPlayerDeath?.Invoke();
    
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
}