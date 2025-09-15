using System;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable, ITargetable
{
    public static event Action OnEnemyDeath;


    private EntityHealth _health;
    


    private void Awake()
    {
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
}