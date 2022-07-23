using UnityEngine;
using UnityEngine.Audio;
using Core.Audio;

namespace Core.Models
{
    [CreateAssetMenu(menuName = "Configuration/Models/Create Audio Settings")]
    public class AudioSettings : ScriptableObject
    {
        [Header("Settings")]
        [SerializeField]
        private MusicBox _musicBox;
        [SerializeField]
        private AudioMixer _audioMixer;

        [Header("Volume")]
        [SerializeField, Range(0f, 1f)]
        private float _globalVolume;
        [SerializeField, Range(0f, 1f)]
        private float _musicVolume;
        [SerializeField, Range(0f, 1f)]
        private float _soundEffects;

        public MusicBox MusicBox => _musicBox;
        public float GlobalVolume => _globalVolume;
        public float MusicVolume => _musicVolume;
        public float SoundEffects => _soundEffects;
    }
}
