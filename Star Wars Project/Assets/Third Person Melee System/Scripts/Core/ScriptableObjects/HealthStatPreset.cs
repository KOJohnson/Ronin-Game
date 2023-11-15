using UnityEngine;

namespace ThirdPersonMeleeSystem.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Health Preset", menuName = "Scriptable Objects/Stats/Health", order = 0)]
    public class HealthStatPreset : ScriptableObject
    {
        [SerializeField] private int maxHealth;

        public int GetMaxHealth()
        {
            return maxHealth;
        }
    }
}