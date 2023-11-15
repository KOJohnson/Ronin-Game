using System;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using ThirdPersonMeleeSystem.Core;
using ThirdPersonMeleeSystem.Managers;
using ThirdPersonMeleeSystem.ScriptableObjects;
using ThirdPersonMeleeSystem.Third_Person_Melee_System.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ThirdPersonMeleeSystem.Player
{
    public class FinisherComponent : MonoBehaviour
    {
        #region Public Fields

        #endregion
    
        #region Private Fields
        
        [ShowInInspector]private List<FinisherTargetComponent> _potentialTargets = new();
        [ShowInInspector]private FinisherTargetComponent _finisherTarget;
        private FinisherAnimationsAsset _finisherAnimationsAsset;

        private bool _canSearchForTargets => currentFinisherPoints >= finisherCost;
        
        #endregion
    
        #region Serialized Fields

        [SerializeField] private FinisherUIComponent finisherUIComponent;
        [SerializeField] private float maxTargetRange;
        [SerializeField] private int finisherCost; //will likely move to scriptable object 
        [SerializeField] private int currentFinisherPoints;
        [SerializeField] private int maxFinisherPoints;
        [Space]
        [SerializeField]private bool OverrideFinisherAnimIndex;
        [ShowIf("OverrideFinisherAnimIndex")][SerializeField] private int index;

        #endregion
    
        #region Getters

        public bool CanTriggerFinisher => currentFinisherPoints >= finisherCost && InputController.FinisherFlag && _finisherTarget;

        #endregion

        private void Start()
        {
            finisherUIComponent.SetFinisherUI(currentFinisherPoints, maxFinisherPoints);
        }

        private void Update()
        {
            if (_canSearchForTargets)
            {
                GetPotentialTargets();
            }

            if (_finisherTarget)
            {
                float distance = Vector3.Distance(transform.position, _finisherTarget.transform.position);
                
                if (distance > maxTargetRange)
                {
                    ClearCurrentTarget();
                }
            }
        }
    
        public void GetPotentialTargets()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, maxTargetRange);

            if (colliders == null)
            {
                _finisherTarget = null;
            }

            for (int i = 0; i < colliders.Length; i++)
            {
                FinisherTargetComponent finisherTarget = colliders[i].GetComponent<FinisherTargetComponent>();
                if (finisherTarget == null) continue;
                if (!finisherTarget.enabled) continue;
                _potentialTargets.Add(finisherTarget);
            }
        
            FindClosestTarget();
        }
    
        private void FindClosestTarget()
        {
            float shortestDistance = Mathf.Infinity;
            Vector3 position = transform.position;
        
            for (int i = 0; i < _potentialTargets.Count; i++)
            {
                float distance = Vector3.Distance(_potentialTargets[i].transform.position, position);

                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    _finisherTarget = _potentialTargets[i];
                }
            }
        
            _potentialTargets.Clear();
        }
        
        public void PerformFinisher()
        {
            GetRandomAnimation();
            RotateTarget(0.15f);
            MovePlayer();
            PlayAnimations();
            SpendFinisherPoints();
        }

        private void SpendFinisherPoints()
        {
            currentFinisherPoints -= finisherCost;
            finisherUIComponent.SetFinisherUI(currentFinisherPoints, maxFinisherPoints);
        }
        
        [Button]
        public void AddFinisherPoints(int amountToAdd = 100)
        {
            currentFinisherPoints += amountToAdd;
            
            if (currentFinisherPoints > maxFinisherPoints)
            {
                currentFinisherPoints = maxFinisherPoints;
            }
            
            finisherUIComponent.SetFinisherUI(currentFinisherPoints, maxFinisherPoints);
        }

        public void ClearCurrentTarget()
        {
            _finisherTarget = null;
        }
        
        private void RotateTarget(float duration)
        {
            transform.DOLookAt(_finisherTarget.transform.position, duration, AxisConstraint.Y);
            _finisherTarget.transform.DOLookAt(transform.position, duration, AxisConstraint.Y);
        }
        
        private void MovePlayer()
        {
            transform.DOMove(
                Helpers.TargetOffset(transform.position,
                    _finisherTarget.transform.position, _finisherAnimationsAsset.finisherAnimation.moveOffset), 0.25f);
        }

        private void PlayAnimations()
        {
            PlayerAnimationManager.Instance.PlayAction(_finisherAnimationsAsset.finisherAnimation.playerFinisherAnimation);
            _finisherTarget.Finisher(_finisherAnimationsAsset.finisherAnimation.enemyFinisherAnimation, 1000);
        }

        private void GetRandomAnimation()
        {
            int random = OverrideFinisherAnimIndex ? 
                index : 
                Random.Range(0, WeaponManager.Instance.GetCurrentWeapon().finisherAnimations.Length);

            _finisherAnimationsAsset = WeaponManager.Instance.GetCurrentWeapon().finisherAnimations[random];
        }

        private void OnDrawGizmos()
        {
            
        }
    }
}
