using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Zenject;
using DG.Tweening;
using Core.Loading;
using Core.UI;

namespace Core.Match
{
    public class SceneController : BaseMenuState, ICleanup
    {
        [SerializeField]
        private AudioListener _listener;
        [SerializeField]
        private EventSystem _eventSystem;
        [SerializeField]
        private RawImage _vignette;

        private ILoadingProvider _loadingProvider;

        public static event Action<Scene> SceneLoaded;

        [Inject]
        private void Construct(ILoadingProvider loadingProvider)
        {
            _loadingProvider = loadingProvider;
        }
        void ICleanup.Cleanup()
        {
            Destroy(_listener);
            Destroy(_eventSystem.gameObject);
        }
        public void SetInteractable(bool isInteractable)
        {
            foreach (var button in FindObjectsOfType<Selectable>())
            {
                button.interactable = isInteractable;
            }
        }
        public async void LoadGameScene_UnityEditor()
        {
            SetInteractable(false);
            _vignette.gameObject.SetActive(true);
            await _vignette.DOFade(1f, 3f).SetEase(Ease.Linear).AsyncWaitForCompletion();

            var operations = new Queue<ILoadingOperation>();
            operations.Enqueue(new SceneCleanupOperation(this));
            operations.Enqueue(new GameLoadingOperation());
            operations.Enqueue(new CreatingBotsOperation());
            operations.Enqueue(new PressAnyButtonOperation());
            await _loadingProvider.LoadAndDestroy(operations);
            SceneLoaded?.Invoke(SceneManager.GetActiveScene());
        }
        public async void Quit_UnityEditor()
        {
            var form = UI.Forms.DecisionForm.CreateForm("Quit", "Are you sure?");
            if (await form.AwaitForConfirm())
            {
                if (!Application.isEditor) Application.Quit();
#if UNITY_EDITOR
                else
                {
                    UnityEditor.EditorApplication.isPlaying = false;
                }
#endif
            }
        }
    }
}