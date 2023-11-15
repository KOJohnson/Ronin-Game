using UnityEngine;
using UnityEngine.UI;

public class HealthBarUIComponent : MonoBehaviour
{
    #region Public Fields
    #endregion
    
    #region Private Fields
    
    
    #endregion
    
    #region Serialized Fields

    [SerializeField] private Image healthBar;
    
    #endregion
    
    #region Getters
    #endregion

    public void SetHealthBarUI(int currentHealth, int maxHealth)
    {
        healthBar.fillAmount = (float)currentHealth / maxHealth;
    }
}
