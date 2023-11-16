using Sirenix.OdinInspector;
using ThirdPersonMeleeSystem.ScriptableObjects;
using UnityEngine;

namespace ThirdPersonMeleeSystem
{
    [RequireComponent(typeof(HealthComponent), typeof(EnemyDamageComponent))]
    public class PostureComponent : MonoBehaviour
    {
        #region Public Fields
        #endregion
    
        #region Private Fields
        
        [ShowInInspector]private int _currentPosture;
        
        #endregion
    
        #region Serialized Fields

        [SerializeField] private PostureStatPreset postureStatPreset;
        [SerializeField] private PostureBarUIComponent postureBarUIComponent;
        
        #endregion
    
        #region Getters
        #endregion

        private void Start()
        {
            _currentPosture = postureStatPreset.GetMaxPosture();
            if (postureBarUIComponent) { postureBarUIComponent.SetPostureBarUI(_currentPosture, postureStatPreset.GetMaxPosture()); }
        }

        public void TakePostureDamage(int damage)
        {
            if (_currentPosture > 0)
            {
                _currentPosture -= damage;
                postureBarUIComponent.SetPostureBarUI(_currentPosture, postureStatPreset.GetMaxPosture());
                Debug.Log($"Hit for: {damage} posture damage!");
            }
            else
            {
                //if current posture <= 0 do posture break stuff
            }
        }

        public int GetCurrentPosture()
        {
            return _currentPosture;
        }
    }
}


