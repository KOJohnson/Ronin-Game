using System;
using UnityEngine;
using UnityEngine.Audio;

namespace ThirdPersonMeleeSystem.ScriptableObjects
{
    [Serializable]
    public class TextureSound
    {
        public Texture texture;
        public AudioResource soundAsset;
        public AudioClip sound;
    }
    
    [CreateAssetMenu(fileName = "New Footstep Sounds", menuName = "Scriptable Objects/Sounds/Footstep Sound Asset", order = 0)]
    public class FootStepSoundsAsset : ScriptableObject
    {
        public TextureSound[] textureSounds;
    }
}