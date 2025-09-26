using System;
using UnityEngine;

namespace Model
{
    public class CopyCatModel
    {
        private readonly int _maxHealth;
        private int _currentHealth;
        private readonly int _healthDrainPerSecond;

        
        public int GetMaxHealth() => _maxHealth;
        public int GetCurrentHealth() => _currentHealth;
        
        public event Action OnDeath;
        public event Action OnHealthUpdate;
        

        public CopyCatModel( int maxHealth, int healthDrainPerSecond)
        {
            _maxHealth = maxHealth;
            _currentHealth = maxHealth;
            _healthDrainPerSecond = healthDrainPerSecond;
        }
        public void DrainHealth()
        {
            TakeDamage(_healthDrainPerSecond);
            Debug.Log($"CopyCat health drained by {_healthDrainPerSecond}");
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