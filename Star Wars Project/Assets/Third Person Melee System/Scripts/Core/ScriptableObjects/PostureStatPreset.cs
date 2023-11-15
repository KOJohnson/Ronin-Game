using UnityEngine;

namespace ThirdPersonMeleeSystem.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Posture Preset", menuName = "Scriptable Objects/Stats/Posture", order = 0)]
    public class PostureStatPreset : ScriptableObject
    {
        [SerializeField] private int maxPosture;

        public int GetMaxPosture()
        {
            return maxPosture;
        }
    }
}