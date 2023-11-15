using ThirdPersonMeleeSystem;
using ThirdPersonMeleeSystem.Interfaces;
using ThirdPersonMeleeSystem.Managers;
using ThirdPersonMeleeSystem.Structs;
using UnityEngine;

public class FinisherTargetComponent : MonoBehaviour,IFinisherTarget
{
    #region Public Fields
    #endregion
    
    #region Private Fields
    #endregion
    
    #region Serialized Fields

    [SerializeField] private AnimationManager animationManager;
    [SerializeField] private HealthComponent healthComponent;
    
    #endregion
    
    #region Getters
    
    
    
    #endregion

    public void Finisher(AnimationData finisherAnimation, int damage)
    {
        enabled = false;
        animationManager.PlayAction(finisherAnimation);
        healthComponent.TakeDamage(damage);
    }
}
