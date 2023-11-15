using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace ThirdPersonMeleeSystem.Third_Person_Melee_System.Scripts
{
    public class FinisherUIComponent : MonoBehaviour
    {
        #region Public Fields
        #endregion
    
        #region Private Fields
        #endregion
    
        #region Serialized Fields

        [SerializeField] private Image finisherUI;
        [SerializeField] private float fillDuration;

        #endregion
    
        #region Getters
    
    
    
        #endregion

        private void Start()
        {
            
        }

        public void SetFinisherUI(int currentFinisherPoints, int maxFinisherPoints)
        {
            float fillAmount = (float)currentFinisherPoints / maxFinisherPoints;
            finisherUI.DOFillAmount(fillAmount, fillDuration);
        }
    }
}