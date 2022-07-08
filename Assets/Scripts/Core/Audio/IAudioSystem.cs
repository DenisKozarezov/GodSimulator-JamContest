using UnityEngine;

namespace Core.Audio
{
    public interface IAudioSystem
    {
        void Play(AudioClip clip);
        void PlayOneShot(AudioClip clip);
        void StartPlayingOST();
        void StopPlayingOST();
        void Fade(FadeMode mode);
    }
}