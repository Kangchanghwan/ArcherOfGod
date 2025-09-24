using System;
using System.Linq;
using Controller.Entity;
using UnityEngine;

public class Enemy : EntityBase, IDamageable, ITargetable
{
    public static event Action OnEnemyDeath;
    private HealthSystem _healthSystem;

    protected override void Awake()
    {
        base.Awake();
        _healthSystem = GetComponent<HealthSystem>();
    }

    // protected override Transform SetTarget() => GameManager.Instance.EnemyOfTarget.GetTransform();


    private void Die()
    {
        OnEnemyDeath?.Invoke();
    }


    public void TakeDamage(float damage)
    {
        // 체력 감소
  
    }


    public Transform GetTransform() => transform;


}