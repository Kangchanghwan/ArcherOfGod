using System;
using UnityEngine;

namespace MVC.Data
{
    public class PlayerModel 
    {
        private readonly int _maxHealth;
        private int _currentHealth;
        
        public int GetMaxHealth() => _maxHealth;
        public int GetCurrentHealth() => _currentHealth;
        
        public event Action OnDeath;
        public event Action OnHealthUpdate;

        public PlayerModel(int maxHealth)
        {
            _maxHealth = maxHealth;
            _currentHealth = maxHealth;
        }

        private void Die() => OnDeath?.Invoke();

        public void UpdateCurrentHealth(int amount)
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