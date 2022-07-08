using UnityEngine;
using Zenject;

namespace Core.Audio
{
    public class AudioSystem : MonoBehaviour, IAudioSystem
    {
        private AudioSourceFactory _factory = new AudioSourceFactory();

        private Models.AudioSettings _settings;
        private bool _isPlaying;

        [Inject]
        public void Construct(Models.AudioSettings settings)
        {
            _settings = settings;
        }

        public void Play(AudioClip clip)
        {
            var source = _factory.Create();
        }
        public void PlayOneShot(AudioClip clip)
        {
            var source = _factory.Create();
            Destroy(source.gameObject, source.clip.length);
        }
        public void StartPlayingOST()
        {
            _isPlaying = true;
            _settings.MusicBox.GetClip();
        }
        public void StopPlayingOST()
        {
            _isPlaying = false;
        }

        public class AudioSourceFactory : IFactory<AudioSource>
        {
            public AudioSource Create()
            {
                var go = new GameObject();
                var clip = go.AddComponent<AudioSource>();
                return clip;
            }
        }
    }
}