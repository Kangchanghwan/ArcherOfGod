using System;
using UnityEngine;
using UnityEngine.UI;

public class EntityHealth : MonoBehaviour, IDamageable
{
    public event Action OnHealthUpdate;
    
    [SerializeField] private Slider healthBar;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;
    
    public bool isDead { get; private set; }
    private bool canTakeDamage = true;

    // 필요한 컴포넌트 참조
    private Entity entity;

    private void Awake()
    {
        entity = GetComponent<Entity>();
    }

    private void Start()
    {
        SetupHealth();
    }

    private void SetupHealth()
    { 
        currentHealth = maxHealth;
        OnHealthUpdate += UpdateHealthBar;

        UpdateHealthBar();
    }

    public virtual bool TakeDamage(float damage)
    {
        if (isDead || canTakeDamage == false)
            return false;
        
        // 체력 감소
        ReduceHealth(damage);
        
        return true;
    }


    private void ReduceHealth(float damage)
    {
        currentHealth = Mathf.Max(0, currentHealth - damage);
        OnHealthUpdate?.Invoke();

        if (currentHealth <= 0)
            Die();
    }

    protected virtual void Die()
    {
        isDead = true;
        entity?.EntityDeath();
    }

    private void UpdateHealthBar()
    {
        if (healthBar == null && healthBar.transform.parent.gameObject.activeSelf == false)
            return;

        healthBar.value = currentHealth / maxHealth;
    }

    
    public void SetCanTakeDamage(bool canTakeDamage) => this.canTakeDamage = canTakeDamage;
    public float GetCurrentHealth() => currentHealth;
    public float GetMaxHealth() =>  maxHealth;
    public float GetHealthPercent() => currentHealth / GetMaxHealth();
    public bool IsDead() => isDead;
}
