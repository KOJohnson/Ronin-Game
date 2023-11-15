using System;
using Animancer;

namespace ThirdPersonMeleeSystem.Structs
{
    [Serializable]
    public class AnimationData : ClipTransition
    {
        public bool UseRootMotion;
        public bool IsInteracting;
        public bool IsSticky;

        public override void Apply(AnimancerState state)
        {
            base.Apply(state);
            state.Root.Component.Animator.applyRootMotion = UseRootMotion;
        }
    }
}