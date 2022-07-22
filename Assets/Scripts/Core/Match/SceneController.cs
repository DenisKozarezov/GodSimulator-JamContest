using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;
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

        private ILoadingProvider _loadingProvider;

        public override void Enter() { }
        public override void Exit() { }

        [Inject]
        public void Construct(ILoadingProvider loadingProvider)
        {
            _loadingProvider = loadingProvider;
        }

        void ICleanup.Cleanup()
        {
            Destroy(_listener);
            Destroy(_eventSystem.gameObject);
        }

        public void LoadGameScene_UnityEditor()
        {
            var operations = new Queue<ILoadingOperation>();
            operations.Enqueue(new SceneCleanupOperation(this));
            operations.Enqueue(new GameLoadingOperation());
            operations.Enqueue(new CreatingBotsOperation());
            operations.Enqueue(new PressAnyButtonOperation());
            _loadingProvider.LoadAndDestroy(operations);
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