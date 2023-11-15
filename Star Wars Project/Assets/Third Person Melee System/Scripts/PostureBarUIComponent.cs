using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PostureBarUIComponent : MonoBehaviour
{
    #region Public Fields
    #endregion
    
    #region Private Fields
    #endregion
    
    #region Serialized Fields
    
    [SerializeField] private Image postureBar;
    
    #endregion
    
    #region Getters
    #endregion
    
    public void SetPostureBarUI(int currentPosture, int maxPosture)
    {
        postureBar.fillAmount = (float)currentPosture / maxPosture;
    }
}
