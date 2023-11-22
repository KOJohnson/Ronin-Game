using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ThirdPersonMeleeSystem.Structs
{
    public enum AttackType
    {
        LightAttack,
        HeavyAttack
    }
    
    [Serializable]
    public struct AttackAnimationData
    {
        public AnimationData attackAnimation;
        public AttackType attackType;
        
        [Header("Overrides")]
        [Space]public bool OverrideDamage;
        [ShowIf("OverrideDamage")]public int damageOverride;
        
        public bool OverrideAnimation;
        [ShowIf("OverrideAnimation")]public AnimationData animationOverride;
    }
}