using System;
using System.Linq;
using UnityEngine;

public class Enemy : EntityBase, IDamageable, ITargetable
{
    public static event Action OnEnemyDeath;
    private EntityHealth _health;

    private Transform _target;
    
    protected override void Awake()
    {
        base.Awake();
        _health = GetComponent<EntityHealth>();
    }

    protected override Transform SetTarget() => GameManager.Instance.EnemyOfTarget.GetTransform();


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


}