using System;
using UnityEngine;

public class CopyCat : EntityBase, IDamageable
{
    public bool IsDead => _health.GetCurrentHealth() <= 0;
    
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


    public void TakeDamage(float damage)
    {
        // 체력 감소
        _health.ReduceHealth(damage);
    }

    public Transform GetTransform() => transform;
}
