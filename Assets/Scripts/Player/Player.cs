using System;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable, ITargetable
{
    public static event Action OnPlayerDeath;

    private PlayerController _playerController;
    private EntityHealth _health;
    
    private void Awake()
    {
        _health = GetComponent<EntityHealth>();
        _playerController = GetComponent<PlayerController>();
    }

    private void Start()
    {
        _playerController.Initialize(_playerController.CastingState);
    }


    private void OnEnable()
    {
        Enemy.OnEnemyDeath += HandlePlayerDeath;
    }
    
    private void OnDisable()
    {
        Enemy.OnEnemyDeath -= HandlePlayerDeath;
        CancelInvoke();
    }

    private void Die()
    {
        OnPlayerDeath?.Invoke();
        _playerController.ChangeState(_playerController.DeadState);
    }

    private void HandlePlayerDeath()
    {
        _playerController.ChangeState(_playerController.IdleState);
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