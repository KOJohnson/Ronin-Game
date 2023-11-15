using ThirdPersonMeleeSystem.Managers;
using UnityEngine;

public class DummyAnimationManager : AnimationManager
{
    #region Public Fields
    #endregion
    
    #region Private Fields
    #endregion
    
    #region Serialized Fields

    [SerializeField] private AnimationClip idleAnimation;

    #endregion

    #region Getters

    #endregion

    private void Update()
    {
        Play(idleAnimation);
    }
}
