using UnityEngine;
using System;

public class EnemyController : MonoBehaviour
{
    #region Public Fields
    
    public event Action deathEvent; 
    
    #endregion
    
    #region Private Fields

    #endregion
    
    #region Serialized Fields
    #endregion
    
    #region Getters
    #endregion

    private void Start()
    {
        
    }

    public void OnDeathEvent()
    {
        deathEvent?.Invoke();
    }
}
