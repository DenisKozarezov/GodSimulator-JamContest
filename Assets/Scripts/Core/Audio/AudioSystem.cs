using System;
using System.Collections;
using UnityEngine;
using Zenject;
using DG.Tweening;

namespace Core.Audio
{
    public class AudioSystem : MonoBehaviour, IAudioSystem
    {
        [SerializeField]
        private AudioSource _musicSource;

        private AudioSourceFactory _factory = new AudioSourceFactory();
        private Models.AudioSettings _settings;
        private bool _isPlaying;
        private bool _isFading;
        private Coroutine _coroutine;

        public event Action MusicEnded;

        [Inject]
        public void Construct(Models.AudioSettings settings)
        {
            _settings = settings;
        }
        private void Awake()
        {
            MusicEnded += OnMusicEnded;
        }
        private void OnDestroy()
        {
            MusicEnded -= OnMusicEnded;
        }
        private void OnMusicEnded()
        {
            _coroutine = StartCoroutine(MusicCoroutine());
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
            if (_settings.MusicBox.Count == 0) return;

            _isPlaying = true;
            OnMusicEnded();
        }
        public void StopPlayingOST()
        {
            StopCoroutine(_coroutine);
            _isPlaying = false;
        }
        private IEnumerator MusicCoroutine()
        {
            var clip = _settings.MusicBox.GetClip();
            _musicSource.clip = clip;
            _musicSource.Play();
            yield return new WaitForSeconds(clip.length);
            MusicEnded?.Invoke();
        }
        public void Fade(FadeMode mode)
        {
            _isFading = true;
            _musicSource.DOFade(mode == FadeMode.Off ? 0f : 1f, 3f).OnComplete(() => _isFading = false);
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