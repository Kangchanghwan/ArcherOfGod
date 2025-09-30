using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace MVC.Data
{
    public class EnemyModel
    {
        private readonly int _maxHealth;
        private int _currentHealth;

        public int GetMaxHealth() => _maxHealth;
        public int GetCurrentHealth() => _currentHealth;
        
        public event Action OnDeath;
        public event Action OnHealthUpdate;


        public EnemyModel(int maxHealth)
        {
            _maxHealth = maxHealth;
            _currentHealth = maxHealth;
        }

        private void Die() => OnDeath?.Invoke();

        public void TakeDamage(int amount)
        {
            var currentHealth = Mathf.Max(_currentHealth - amount, 0);
         
            if(currentHealth == _currentHealth) return;
            
            _currentHealth = currentHealth;
            OnHealthUpdate?.Invoke();
            
            if (_currentHealth <= 0)
                Die();

        }

    }
}