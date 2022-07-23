using UnityEngine;

namespace Core.Models
{
    [CreateAssetMenu(menuName = "Configuration/Models/Create UI Settings")]
    public class UISettings : ScriptableObject
    {
        [Header("Settings")]
        [SerializeField, Range(0f, 15f)]
        private float _outlineWidth;
        [SerializeField]
        private bool _autoFadeWhenGameStart;
        [SerializeField]
        private bool _autoFadeWhenSceneTransition;
        [SerializeField, Min(0f)]
        private float _fadeDuration;

        public float OutlineWidth => _outlineWidth;
        public bool AutoFadeWhenGameStart => _autoFadeWhenGameStart;
        public bool AutoFadeWhenSceneTransition => _autoFadeWhenSceneTransition;
        public float FadeDuration => _fadeDuration;
    }
}