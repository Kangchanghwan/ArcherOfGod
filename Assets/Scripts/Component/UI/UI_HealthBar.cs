using System;
using Controller.Entity;
using MVC.Controller;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace UI
{
    public class UI_HealthBar : MonoBehaviour
    {
    
        [SerializeField] private Slider healthBar;
        [SerializeField] private EntityType type;
        
        private void OnEnable()
        {
            EventManager.Subscribe<OnHealthUpdateEvent>(OnUpdateUI);
        }

        private void OnDisable()
        {
            EventManager.Unsubscribe<OnHealthUpdateEvent>(OnUpdateUI);
        }

        private void OnUpdateUI(OnHealthUpdateEvent @event)
        {
            if(@event.Type != type) return;
            
            UpdateHealthBar(@event.CurrentHealth, @event.MaxHealth);
        }

        public void UpdateHealthBar(float currentHealth, float maxHealth)
        {
            if (healthBar == null && healthBar.transform.parent.gameObject.activeSelf == false)
                return;

            healthBar.value = currentHealth / maxHealth;
        }
        
    }
}
