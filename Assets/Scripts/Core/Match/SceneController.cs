using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Zenject;
using DG.Tweening;
using Core.Loading;
using Core.UI;
using Core.Models;

namespace Core.Match
{
    public class SceneController : BaseMenuState, ICleanup
    {
        [SerializeField]
        private RawImage _fade;

        private UISettings _settings;
        private ILoadingProvider _loadingProvider;

        public static event Action<Scene> SceneLoaded;

        [Inject]
        private void Construct(UISettings UISettings, ILoadingProvider loadingProvider)
        {
            _settings = UISettings;
            _loadingProvider = loadingProvider;
        }
        void ICleanup.Cleanup()
        {
            foreach (var listener in FindObjectsOfType<AudioListener>())
            {
                Destroy(listener.gameObject);
            }
            foreach (var system in FindObjectsOfType<EventSystem>())
            {
                Destroy(system.gameObject);
            }
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

            if (_fade != null && _settings.AutoFadeWhenSceneTransition)
            {
                _fade.gameObject.SetActive(true);
                await _fade.DOFade(1f, _settings.FadeDuration).SetEase(Ease.Linear).AsyncWaitForCompletion();
            }

            Cursor.visible = false;

            var operations = new Queue<LazyLoadingOperation>();
            Func<ILoadingOperation> sceneCleanup = () => new SceneCleanupOperation(this);
            Func<ILoadingOperation> gameLoad = () => new GameLoadingOperation();
            Func<ILoadingOperation> creatingBots = () => new CreatingBotsOperation { Count = 3 };
            Func<ILoadingOperation> pressAnyButton = () => new PressAnyButtonOperation();
            operations.Enqueue(sceneCleanup);
            operations.Enqueue(gameLoad);
            operations.Enqueue(creatingBots);
            operations.Enqueue(pressAnyButton);
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