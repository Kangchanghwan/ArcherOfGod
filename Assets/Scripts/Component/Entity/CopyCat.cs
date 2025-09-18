using System;
using UnityEngine;

public class CopyCat : EntityBase, IDamageable
{
    
    [SerializeField]
    private int healthDrainPerSecond;
    private float _timer;
    
    private EntityHealth _health;
    private CopyCatStateMachine _stateMachine;
    protected override void Awake()
    {
        base.Awake();
        _health = GetComponent<EntityHealth>();
        _stateMachine = GetComponent<CopyCatStateMachine>();
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
    private void Die()
    {
        _stateMachine.OnChangeFadeOutState();
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
