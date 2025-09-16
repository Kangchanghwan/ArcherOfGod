using System;
using UnityEngine;

public class Player : EntityBase, IDamageable, ITargetable
{
    public static event Action OnPlayerDeath;
    private EntityHealth _health;

    protected override void Awake()
    {
        base.Awake();
        _health = GetComponent<EntityHealth>();
    }
    protected override Transform SetTarget() => GameManager.Instance.PlayerOfTarget.GetTransform();

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
    
}