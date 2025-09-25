using System;
using UnityEngine;

namespace Model
{
    public class CopyCatModel
    {
        private int _attack;
        private readonly int _maxHealth;
        private int _currentHealth;
        private readonly int _healthDrainPerSecond;

        
        public int GetMaxHealth() => _maxHealth;
        public int GetCurrentHealth() => _currentHealth;
        
        public event Action OnCopyCatDeath;
        

        public CopyCatModel(int attack, int maxHealth, int healthDrainPerSecond)
        {
            _attack = attack;
            _maxHealth = maxHealth;
            _currentHealth = maxHealth;
            _healthDrainPerSecond = healthDrainPerSecond;
        }
        public void DrainHealth()
        {
            TakeDamage(_healthDrainPerSecond);
            Debug.Log($"CopyCat health drained by {_healthDrainPerSecond}");
        }
        
        private void Die() => OnCopyCatDeath?.Invoke();

        public void TakeDamage(int amount)
        {
            var currentHealth = Mathf.Max(_currentHealth - amount, 0);
         
            if(currentHealth == _currentHealth) return;
            
            _currentHealth = currentHealth;
            if (_currentHealth <= 0)
                Die();
        }

    }
}