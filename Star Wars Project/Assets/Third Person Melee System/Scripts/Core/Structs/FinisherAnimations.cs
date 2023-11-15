using System;
using UnityEngine;

namespace ThirdPersonMeleeSystem.Structs
{
    [Serializable]
    public struct FinisherAnimations
    {
        public AnimationData playerFinisherAnimation;
        [Space]public AnimationData enemyFinisherAnimation;
        [Space]public float moveOffset;
    }
}