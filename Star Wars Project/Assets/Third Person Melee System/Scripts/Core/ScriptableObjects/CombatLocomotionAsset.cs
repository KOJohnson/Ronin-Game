using Animancer;
using ThirdPersonMeleeSystem.Structs;
using UnityEngine;

namespace ThirdPersonMeleeSystem.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Combat Locomotion Asset", menuName = "Scriptable Objects/Animations/CombatLocomotionAsset", order = 0)]
    public class CombatLocomotionAsset : ScriptableObject
    {
        [Header("Mixers")]
        [SerializeField] private LinearMixerTransitionAsset.UnShared locomotionMixer;
        [SerializeField] private MixerTransition2DAsset.UnShared directionalWalkMixer;
        [SerializeField] private MixerTransition2DAsset.UnShared directionalJogMixer;
        [SerializeField] private LinearMixerTransitionAsset.UnShared blockingLinearMixer;
        [SerializeField] private MixerTransition2DAsset.UnShared blockingDirectionalMixer;
        
        [Header("Block Animations")]
        public AnimationData blockStart;
        public AnimationData blockRelease;
        
        [Header("Jump Animations")] 
        public AnimationData combatJumpStart;
        public AnimationData combatJumpLoop;
        public AnimationData combatJumpEnd;
        
        [Header("Dodge Animations")]
        public AnimationData dodgeForward;
        public AnimationData dodgeBackward;
        public AnimationData dodgeLeft;
        public AnimationData dodgeRight;
        
        [Header("Roll Animations")]
        public AnimationData rollForward;
        public AnimationData rollBackward;
        public AnimationData rollLeft;
        public AnimationData rollRight;

        public LinearMixerTransitionAsset.UnShared GetLocomotionMixer()
        {
            return locomotionMixer;
        }
        
        public MixerTransition2DAsset.UnShared GetDirectionalWalkMixer()
        {
            return directionalWalkMixer;
        }
        
        public MixerTransition2DAsset.UnShared GetDirectionalJogMixer()
        {
            return directionalJogMixer;
        }
    }
}