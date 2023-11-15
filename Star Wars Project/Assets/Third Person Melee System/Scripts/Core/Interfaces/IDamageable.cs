using ThirdPersonMeleeSystem.Structs;
using UnityEngine;

namespace ThirdPersonMeleeSystem.Interfaces
{
    public interface IDamageable
    {
        public void TakeDamage(Vector3 attackerPos, int damage, int postureDamage, AttackType attackType);
    }
}