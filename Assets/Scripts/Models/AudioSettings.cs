using Core.Audio;
using UnityEngine;
using UnityEngine.Audio;

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
        private float _effectsVolume;

        public MusicBox MusicBox => _musicBox;
        public float GlobalVolume => _globalVolume;
        public float EffectsVolume => _effectsVolume;
    }
}
