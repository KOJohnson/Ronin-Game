using ThirdPersonMeleeSystem.Structs;
using UnityEngine;

namespace ThirdPersonMeleeSystem.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Hit Reaction Set", menuName = "Scriptable Objects/Animations/Hit Reactions", order = 0)]
    public class HitReactionAnimation : ScriptableObject
    {
        public AnimationData[] hitFrontReactions;
        public AnimationData[] hitBackReactions;
    }
}