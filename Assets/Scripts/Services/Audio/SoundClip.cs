using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Services.Audio
{
    [Serializable]
    [RequireComponent(typeof(AudioSource))]
    public class SoundClip
    {
        public string AudioName;
        public AudioClip Clip;
        public bool Loop;

        [Range(0, 1f)] 
        public float Volume = 1;
        
        [Range(.1f, 3f)]
        public float Pitch = 1;

        [HideInInspector]
        public AudioSource Source;
    }
}