using UnityEngine;
using System.Collections.Generic;

namespace Services.Audio
{
    public class AudioManager : MonoBehaviour
    {
        private readonly Dictionary<string, AudioSource> _soundDictionary = new();

        [SerializeField] private SoundClip[] _soundClips;

        private void Awake()
        {
            foreach (var sound in _soundClips)
            {
                var source = gameObject.AddComponent<AudioSource>();
                source.clip = sound.Clip;
                source.volume = sound.Volume;
                source.pitch = sound.Pitch;
                source.loop = sound.Loop;
                _soundDictionary[sound.AudioName] = source;
            }
        }

        private void Start()
        {
            PlayMainTheme();
        }

        public void Play(string soundName)
        {
            if (_soundDictionary.TryGetValue(soundName, out var sound))
            {
                sound.Play();
            }
            else
            {
                Debug.LogWarning("Sound with name " + soundName + " does not exist.");
            }
        }

        private void PlayMainTheme()
        {
            _soundDictionary["MainTheme"].Play();
        }
        
        public void PlayButtonClickSound()
        {
            _soundDictionary["ButtonClick"].Play();
        }

        public void PlayButtonHoverSound()
        {
            _soundDictionary["ButtonHover"].Play();
        }

        public void PlayItemClickSound()
        {
            _soundDictionary["ItemClick"].Play();
        }

        public void PlayItemHoverSound()
        {
            _soundDictionary["ItemHover"].Play();
        }

        public void PlayThreeCollectedSound()
        {
            _soundDictionary["3ItemsMatch"].Play();
        }

        public void PlayWinSound()
        {
            _soundDictionary["Win"].Play();
        }

        public void PlayLoseSound()
        {
            _soundDictionary["Loss"].Play();
        }
        
        public void PlayGoalSound()
        {
            _soundDictionary["GoalCollected"].Play();
        }

        public void PlayFanSound()
        {
            _soundDictionary["Fan"].Play();
        }
    }
}