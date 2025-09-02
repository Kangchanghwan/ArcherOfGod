using System;
using UnityEngine;
using UnityEngine.UI;

public class EntityHealth : MonoBehaviour
{
    public event Action OnHealthUpdate;
    
    [SerializeField] private Slider healthBar;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;
    
    
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


    public void ReduceHealth(float damage)
    {
        currentHealth = Mathf.Max(0, currentHealth - damage);
        OnHealthUpdate?.Invoke();
    }


    private void UpdateHealthBar()
    {
        if (healthBar == null && healthBar.transform.parent.gameObject.activeSelf == false)
            return;

        healthBar.value = currentHealth / maxHealth;
    }

    
    public float GetCurrentHealth() => currentHealth;
    public float GetMaxHealth() =>  maxHealth;
}
