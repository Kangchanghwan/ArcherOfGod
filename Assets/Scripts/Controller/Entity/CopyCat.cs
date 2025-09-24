using System;
using Controller.Entity;
using UnityEngine;

public class CopyCat : EntityBase, IDamageable
{
    // public bool IsDead => _healthSystem.GetCurrentHealth() <= 0;
    
    [SerializeField]
    private int healthDrainPerSecond;
    private float _timer;
    
    private HealthSystem _healthSystem;
    
    protected override void Awake()
    {
        base.Awake();
        _healthSystem = GetComponent<HealthSystem>();
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

    // protected override Transform SetTarget() => GameManager.Instance.PlayerOfTarget.GetTransform();


    public void TakeDamage(float damage)
    {
        // 체력 감소
    }

    public Transform GetTransform() => transform;
}
