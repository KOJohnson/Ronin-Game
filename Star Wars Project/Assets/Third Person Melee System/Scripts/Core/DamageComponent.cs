using UnityEngine;
using UnityEngine.Events;
using ThirdPersonMeleeSystem.Interfaces;
using ThirdPersonMeleeSystem.Structs;

namespace ThirdPersonMeleeSystem
{
    [RequireComponent(typeof(HealthComponent))]
    public class DamageComponent : MonoBehaviour,IDamageable
    {
        #region Public Fields
        
        
        #endregion
    
        #region Private Fields

        #endregion
    
        #region Serialized Fields

        [SerializeField] private UnityEvent OnTakeDamageEvent;
        
        #endregion
    
        #region Getters

        
        
        #endregion

        private void Start()
        {
        }

        public void TakeDamage(Vector3 attackerPos, int damage, int postureDamage, AttackType attackType)
        {
            OnTakeDamageEvent?.Invoke(); //Do damage
        }

    }
}

