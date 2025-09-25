using System;
using UnityEngine;

namespace Model
{
    public class PlayerModel
    {
        private int _attack;
        private readonly int _maxHealth;
        private int _currentHealth;
        
        public int GetMaxHealth() => _maxHealth;
        public int GetCurrentHealth() => _currentHealth;
        
        public static event Action OnPlayerDeath;

        public PlayerModel(int attack, int maxHealth)
        {
            _attack = attack;
            _maxHealth = maxHealth;
            _currentHealth = maxHealth;
        }

        private void Die() => OnPlayerDeath?.Invoke();

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