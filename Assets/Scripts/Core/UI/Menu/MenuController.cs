using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;
using Zenject;
using Core.Input;

namespace Core.UI
{
    public enum MenuStates : byte
    {
        Main = 0x00,
        Settings = 0x01,
        Credits = 0x02,
        Multiplayer = 0x10,
    }

    public class MenuController : MonoBehaviour, IStateMachine<MenuController>
    {
        [SerializeField]
        private SerializableDictionaryBase<MenuStates, BaseMenuState> _states = new SerializableDictionaryBase<MenuStates, BaseMenuState>();

        private MenuStates _currentState;
        private IInputSystem _inputSystem;
        private ILogger _logger;
        private readonly Stack<MenuStates> Stack = new Stack<MenuStates>();

        public IState<MenuController> CurrentState => _states[_currentState];

        [Inject]
        public void Construct(ILogger logger, IInputSystem inputSystem)
        {
            _inputSystem = inputSystem;
            _logger = logger;
        }
        private void Start()
        {
            _inputSystem.Escape += GetBack;
            SwitchState(MenuStates.Main);
        }
        private void OnDestroy()
        {
            _inputSystem.Escape -= GetBack;
        }
        public void GetBack()
        {
            if (_currentState == MenuStates.Main) return;

            CloseLastWindow();
        }
        public void SwitchState(MenuStates state)
        {
            _currentState = state;
            Stack.Push(state);
            if (_states.TryGetValue(state, out var menuState))
            {
                SwitchState(menuState);            
            }

#if UNITY_EDITOR
            _logger.Log($"<b><color=yellow>Menu Controller</color></b> switching to state <b><color=yellow>{state}</color></b>.", LogType.Game);
#endif
        }
        public void SwitchState<T>(T state) where T : IState<MenuController>
        {
            CurrentState?.Exit();
            state?.Enter();
        }
        public void CloseLastWindow()
        {
            if (Stack.Count <= 1)
            {
                SwitchState(MenuStates.Main);
            }
            else
            {
                SwitchState(Stack.Pop());
            }
        }
    }
}
