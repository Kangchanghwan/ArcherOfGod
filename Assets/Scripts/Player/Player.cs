using System;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable, ITargetable
{
    public static event Action OnPlayerDeath;

    private PlayerController _playerController;
    public EntityHealth health;
    

    [Header("Movement")] [SerializeField] public float moveSpeed = 5f;
    private bool _manualRotation;

    public ITargetable target;

    public float xInput { get; private set; }

    private void Awake()
    {
        health = GetComponent<EntityHealth>();
        
        _playerController = GetComponent<PlayerController>();
    }

    private void Start()
    {
        _playerController.Initialize(_playerController.CastingState);
        target = GameManager.instance.PlayerOfTarget;
    }


    private void OnEnable()
    {
        InputManager input = InputManagerSingleton.Instance.InputManager;
        input.Enable();

        input.Player.Move.performed += ctx => xInput = ctx.ReadValue<float>();
        input.Player.Move.canceled += _ => xInput = 0f;

        Enemy.OnEnemyDeath += HandlePlayerDeath;
    }


    private void OnDisable()
    {
        
        InputManagerSingleton.Instance.InputManager.Disable();
        Enemy.OnEnemyDeath -= HandlePlayerDeath;
        CancelInvoke();
    }

    private void Die()
    {
        OnPlayerDeath?.Invoke();
        _playerController.ChangeState(_playerController.DeadState);
    }

    public void ActivateManualRotation(bool manualRotation) => _manualRotation = manualRotation;

    public bool ManualRotationActive() => _manualRotation;

    private void HandlePlayerDeath()
    {
        _playerController.ChangeState(_playerController.IdleState);
    }

    public void TakeDamage(float damage)
    {
        // 체력 감소
        health.ReduceHealth(damage);

        if (health.GetCurrentHealth() <= 0)
        {
            Die();
        }
    }

    public Transform GetTransform() => transform;
}