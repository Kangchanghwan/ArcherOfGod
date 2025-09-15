using System;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable, ITargetable
{
    public static event Action OnPlayerDeath;
    
    private EntityHealth _health;


    private void Awake()
    {
        _health = GetComponent<EntityHealth>();
    }


    private void Die()
    {
        OnPlayerDeath?.Invoke();
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