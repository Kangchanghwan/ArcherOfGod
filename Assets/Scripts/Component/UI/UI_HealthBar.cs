using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UI_HealthBar : MonoBehaviour
    {
    
        [SerializeField] private Slider healthBar;
    

        public void UpdateHealthBar(float currentHealth, float maxHealth)
        {
            if (healthBar == null && healthBar.transform.parent.gameObject.activeSelf == false)
                return;

            healthBar.value = currentHealth / maxHealth;
        }

    }
}
