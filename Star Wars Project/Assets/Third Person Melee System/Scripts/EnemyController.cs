using UnityEngine;
using System;
using ThirdPersonMeleeSystem;

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
    
    [field:SerializeField] public LockOnTarget LockOnTarget { get; set;}
    
    
    #endregion

    private void OnEnable()
    {
        EnemyCombatManager.Instance.AddToList(this);
    }

    private void OnDisable()
    {
        EnemyCombatManager.Instance.RemoveFromList(this);
    }

    public void OnDeathEvent()
    {
        deathEvent?.Invoke();
    }
}
