using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Core.Loading;
using Core.UI;

namespace Core.Match
{
    public class SceneController : BaseMenuState
    {
        private ILoadingProvider _loadingProvider;

        public override void Enter() { }
        public override void Exit() { }

        [Inject]
        public void Construct(ILoadingProvider loadingProvider)
        {
            _loadingProvider = loadingProvider;
        }

        public void LoadGameScene_UnityEditor()
        {
            var operations = new Queue<ILoadingOperation>();
            operations.Enqueue(new GameLoadingOperation());
            operations.Enqueue(new CreatingBotsOperation());
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