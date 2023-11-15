using System;
using UnityEngine;

namespace ThirdPersonMeleeSystem.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New WeaponEvent", menuName = "Scriptable Objects/Weapons/WeaponEvents", order = 0)]
    public class WeaponEvents : ScriptableObject
    {
        public event Action<bool> ToggleWeaponEvent;
        public event Action<bool> ToggleCanCombo;
        public event Action<bool> ToggleDamageComponent;

        public void OnToggleWeaponEvent(bool toggle)
        {
            ToggleWeaponEvent?.Invoke(toggle);
        }

        public void OnEnableCombo(bool toggle)
        {
            ToggleCanCombo?.Invoke(toggle);
        }

        public void OnToggleDamageComponent(bool toggle)
        {
            ToggleDamageComponent?.Invoke(toggle);
        }
    }
}