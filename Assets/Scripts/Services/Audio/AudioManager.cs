using System;
using UnityEngine;

namespace Services.Audio
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private SoundClip[] _soundClips;

        private void Awake()
        {
            foreach (var sound in _soundClips)
            {
                sound.Source = gameObject.AddComponent<AudioSource>();
                sound.Source.clip = sound.Clip;
                sound.Source.volume = sound.Volume;
                sound.Source.pitch = sound.Pitch;
                sound.Source.loop = sound.Loop;
            }
        }

        private void Start()
        {
            Play("MainTheme");
        }

        public void Play(string soundName)
        {
            var sound = Array.Find(_soundClips, sound => sound.AudioName == soundName);
            sound.Source.Play();
        }
    }
}