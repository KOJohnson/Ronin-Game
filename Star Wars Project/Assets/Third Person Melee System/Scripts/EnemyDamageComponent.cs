using ThirdPersonMeleeSystem.Interfaces;
using ThirdPersonMeleeSystem.Managers;
using ThirdPersonMeleeSystem.ScriptableObjects;
using ThirdPersonMeleeSystem.Structs;
using UnityEngine;
using UnityEngine.Events;

namespace ThirdPersonMeleeSystem
{
    public class EnemyDamageComponent : MonoBehaviour,IDamageable
    {
        #region Public Fields
        
        
        #endregion
    
        #region Private Fields

        #endregion
    
        #region Serialized Fields
    
        [SerializeField] private AnimationManager animationManager;
        [SerializeField] private HealthComponent healthComponent;
        [SerializeField] private PostureComponent postureComponent;
        [SerializeField] private HitReactionAnimation hitReactionData;
    
        [SerializeField] private UnityEvent OnTakeDamageEvent;
        [SerializeField] private UnityEvent OnBlockEvent;
        [SerializeField] private UnityEvent OnTakePostureDamageEvent;
        
        [Header("Tests")] 
        [SerializeField] private bool isBlocking;
    
        #endregion
    
        #region Getters
        
        
        #endregion

        private void Start()
        {
        }

        public void TakeDamage(Vector3 attackerPos, int damage, int postureDamage, AttackType attackType)
        {
            if (isBlocking)
            {
                switch (attackType)
                {
                    case AttackType.LightAttack:
                        OnBlockEvent?.Invoke(); //Attack Blocked
                        break;
                    case AttackType.HeavyAttack:
                        OnTakePostureDamageEvent?.Invoke(); //Do posture damage
                        if (postureComponent) postureComponent.TakePostureDamage(postureDamage);
                        break;
                }
            }
            else
            {
                OnTakeDamageEvent?.Invoke(); //Do damage
                if (healthComponent) healthComponent.TakeDamage(damage);
                if (animationManager) animationManager.PlayAction(PlayHitReaction(attackerPos));
            }
        }

        private AnimationData PlayHitReaction(Vector3 attacker)
        {
            return GetRelativePosition(attacker).z > 0 ? hitReactionData.hitFrontReactions[0] : hitReactionData.hitBackReactions[0];
        }

        private Vector3 GetRelativePosition(Vector3 attacker)
        {
            return transform.InverseTransformPoint(attacker);
        }
    
    }
}


