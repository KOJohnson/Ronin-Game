using System;
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
        
        [Header("Refs")]
        [SerializeField] private EnemyController enemyController;
        [SerializeField] private AnimationManager animationManager;
        [SerializeField] private HealthComponent healthComponent;
        [SerializeField] private PostureComponent postureComponent;
        [SerializeField] private HitReactionAnimation hitReactionData;

        [Header("Damage Collider")] 
        [SerializeField] private Collider damageCollider;
        
        [Header("Unity Events")]
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

        private void OnEnable()
        {
            enemyController.deathEvent += DisableDamageCollider;
            enemyController.deathEvent += () => gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            enemyController.deathEvent -= DisableDamageCollider;
        }

        public void TakeDamage(Vector3 attackerPos, int damage, int postureDamage, AttackType attackType)
        {
            if (isBlocking)
            {
                switch (attackType)
                {
                    case AttackType.LightAttack:
                        OnBlockEvent?.Invoke();
                        break;
                    case AttackType.HeavyAttack:
                        OnTakePostureDamageEvent?.Invoke();
                        postureComponent.TakePostureDamage(postureDamage);
                        break;
                }
            }
            else
            {
                OnTakeDamageEvent?.Invoke();
                healthComponent.TakeDamage(damage);
                animationManager.PlayAction(PlayHitReaction(attackerPos));
            }

            if (healthComponent.IsDead())
            {
                Debug.Log("is dead is " + healthComponent.IsDead());
                enemyController.OnDeathEvent();
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

        private void DisableDamageCollider()
        {
            damageCollider.enabled = false;
        }
    
    }
}


