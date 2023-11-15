using System;
using UnityEngine;

namespace ThirdPersonMeleeSystem.ScriptableObjects
{
    [Serializable]
    public class TextureSound
    {
        public Texture texture;
        public AudioClip sound;
    }
    
    [CreateAssetMenu(fileName = "New Footstep Sounds", menuName = "Scriptable Objects/Sounds/Footstep Sound Asset", order = 0)]
    public class FootStepSoundsAsset : ScriptableObject
    {
        public TextureSound[] textureSounds;
    }
}