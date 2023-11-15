using Animancer;
using ThirdPersonMeleeSystem.Structs;
using UnityEngine;

namespace ThirdPersonMeleeSystem.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Locomotion Asset", menuName = "Scriptable Objects/Animations/LocomotionAsset", order = 0)]
    public class LocomotionAsset : ScriptableObject
    {
        [SerializeField] private LinearMixerTransitionAsset.UnShared locomotionMixer;
        
        [Header("Start Animations")]
        public AnimationData walkStart;
        public AnimationData jogStart;
        public AnimationData runStart;
        
        [Header("Stop Animations")]
        public AnimationData walkStop;
        public AnimationData jogStop;
        public AnimationData runStop;

        [Header("Jump Animations")] 
        public AnimationData jumpStart;
        public AnimationData jumpLoop;
        public AnimationData jumpEnd;
        
        public AnimationData slideStart;
        public AnimationData slideLoop;
        public AnimationData slideEnd;

        public LinearMixerTransitionAsset.UnShared GetLocomotionMixer()
        {
            return locomotionMixer;
        }
    }
}