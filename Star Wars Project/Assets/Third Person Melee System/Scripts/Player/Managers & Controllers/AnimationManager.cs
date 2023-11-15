using Animancer;
using ThirdPersonMeleeSystem.Structs;
using UnityEngine;

namespace ThirdPersonMeleeSystem.Managers
{
    public class AnimationManager : MonoBehaviour
    {
        #region Public Fields
        
        
        #endregion
    
        #region Private Fields

        protected AnimancerLayer _locomotionLayer;
        protected AnimancerLayer _actionLayer;

        #endregion
    
        #region Serialized Fields

        [field:SerializeField] public HybridAnimancerComponent AnimancerComponent { get; private set; }
        [SerializeField] private float layerFadeDuration;

        #endregion
    
        #region Getters
        
        public bool IsInteracting { get; private set; }
        public bool UseRootMotion { get; private set; }

        #endregion

        protected virtual void Start()
        {
            _locomotionLayer = AnimancerComponent.Layers[0];
            _actionLayer = AnimancerComponent.Layers[1];
        }

        public void Play(AnimationClip motion)
        {
            if (motion == null) return;
            _locomotionLayer.Play(motion);
        }

        public void PlayAction(AnimationData motion)
        {
            if (motion.Clip == null) return;
            IsInteracting = motion.IsInteracting;
            UseRootMotion = motion.UseRootMotion;
            AnimancerState state = _actionLayer.Play(motion);
            state.Events.OnEnd += OnActionEnd;
        }

        public void Play(string stateName, bool isInteracting)
        {
            IsInteracting = isInteracting;
            AnimancerComponent.Animator.Play(stateName);
        }

        protected void OnActionEnd()
        {
            ResetModifiers();
            _actionLayer.StartFade(0f, layerFadeDuration);
        }

        private void ResetModifiers()
        {
            IsInteracting = false;
            UseRootMotion = false;
        }
    }
}
