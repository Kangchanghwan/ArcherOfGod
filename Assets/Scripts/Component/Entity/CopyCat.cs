using System;
using UnityEngine;

public class CopyCat : EntityBase, IDamageable
{
    public static event Action OnCopyCatDeath;
    
    [SerializeField]
    private int healthDrainPerSecond;
    private float _timer;
    
    private EntityHealth _health;
    
    protected override void Awake()
    {
        base.Awake();
        _health = GetComponent<EntityHealth>();
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer > 1f)
        {
            TakeDamage(healthDrainPerSecond);
            _timer = 0f; 
        }
    }

    protected override Transform SetTarget() => GameManager.Instance.PlayerOfTarget.GetTransform();
    private void Die() => OnCopyCatDeath?.Invoke();
    
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
