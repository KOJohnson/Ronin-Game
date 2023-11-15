using UnityEngine;

namespace ThirdPersonMeleeSystem.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Weapon Sounds", menuName = "Scriptable Objects/Weapons/Weapon Sounds", order = 0)]
    public class WeaponSounds : ScriptableObject
    {
        public AudioClip[] weaponSwingSound;
    }
}