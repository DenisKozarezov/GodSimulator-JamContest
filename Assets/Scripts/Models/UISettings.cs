using UnityEngine;
using Core.UI;

namespace Core.Models
{
    [CreateAssetMenu(menuName = "Configuration/Models/Create UI Settings")]
    public class UISettings : ScriptableObject
    {
        [Header("Settings")]
        [SerializeField, Range(0f, 15f)]
        private float _outlineWidth;

        [Header("Cursor")]
        [SerializeField]
        private CursorSize _cursorSize;
        [SerializeField]
        private CursorMode _cursorMode;

        [Header("Fade")]
        [SerializeField]
        private bool _autoFadeWhenGameStart;
        [SerializeField]
        private bool _autoFadeWhenSceneTransition;
        [SerializeField, Min(0f)]
        private float _fadeDuration;

        public float OutlineWidth => _outlineWidth;
        public Vector2 CursorSize
        {
            get
            {
                switch (_cursorSize)
                {
                    case UI.CursorSize._16x16: return Vector2.one * 16f;
                    case UI.CursorSize._32x32: return Vector2.one * 32f;
                    case UI.CursorSize._64x64: return Vector2.one * 64f;
                    case UI.CursorSize._128x128: return Vector2.one * 128f;
                    default: return Vector2.one * 64f;
                }
            }
        }
        public CursorMode CursorMode => _cursorMode;
        public bool AutoFadeWhenGameStart => _autoFadeWhenGameStart;
        public bool AutoFadeWhenSceneTransition => _autoFadeWhenSceneTransition;
        public float FadeDuration => _fadeDuration;
    }
}