using ThirdPersonMeleeSystem.ScriptableObjects;
using UnityEngine;

namespace ThirdPersonMeleeSystem
{
    public class HealthComponent : MonoBehaviour
    {
        #region Public Fields
        
        
        #endregion
    
        #region Private Fields

        private int _currentHealth;    
    
        #endregion
    
        #region Serialized Fields

        [SerializeField] private HealthStatPreset healthStatPreset;
        [SerializeField] private HealthBarUIComponent healthBarUI;
    
        #endregion
    
        #region Getters
        #endregion

        private void Start()
        {
            _currentHealth = healthStatPreset.GetMaxHealth();
            if (healthBarUI) healthBarUI.SetHealthBarUI(_currentHealth, healthStatPreset.GetMaxHealth());
        }

        public void TakeDamage(int damage)
        {
            if (_currentHealth <= 0){ return; }
            
            _currentHealth -= damage;
            if (healthBarUI) healthBarUI.SetHealthBarUI(_currentHealth, healthStatPreset.GetMaxHealth());
            
            Debug.Log($"Hit for: {damage} damage!");
            //if current health <= 0 do death stuff
        }
    }
}

