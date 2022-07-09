using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
using DG.Tweening;

namespace Core
{
    public class CameraScript : MonoBehaviour
    {
        [SerializeField]
        private PostProcessVolume _volume;
        [SerializeField]
        private RawImage _fade;

        private T GetSettings<T>() where T : PostProcessEffectSettings
        {
            if (_volume.profile.TryGetSettings<T>(out var setting))
            {
                return setting;
            }
            return null;
        }

        public void Fade(FadeMode mode, float time)
        {
            Color endColor = mode == FadeMode.Off ? Color.black : _fade.color.SetAlpha(0f);
            _fade.DOColor(endColor, time);
        }
    }
}