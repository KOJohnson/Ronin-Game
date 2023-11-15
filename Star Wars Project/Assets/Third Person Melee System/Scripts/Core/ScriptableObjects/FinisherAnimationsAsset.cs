using ThirdPersonMeleeSystem.Structs;
using UnityEngine;

namespace ThirdPersonMeleeSystem.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Finisher Asset", menuName = "Scriptable Objects/Attacks/Finisher Asset", order = 0)]
    public class FinisherAnimationsAsset : ScriptableObject
    {
        public FinisherAnimations finisherAnimation;
    }
}