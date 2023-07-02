using System;
using UnityEngine;

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
        public float Volume;
        
        [Range(.1f, 3f)]
        public float Pitch;

        [HideInInspector]
        public AudioSource Source;
    }
}