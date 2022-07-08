using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;

namespace Core.UI
{
    public enum MenuState : byte
    {
        Main = 0x00,
        Settings = 0x01,
        Credits = 0x02,
    }

    public class MenuController : MonoBehaviour, IStateMachine<MenuController>
    {
        [SerializeField]
        private SerializableDictionaryBase<MenuState, BaseMenuState> States = new SerializableDictionaryBase<MenuState, BaseMenuState>();

        private MenuState _currentState;
        public IState<MenuController> CurrentState => States[_currentState];

        private void Start()
        {
            SwitchState(MenuState.Main);
        }
        public void GetBack()
        {

        }

        public void SwitchState(MenuState state)
        {
            _currentState = state;

            if (States.TryGetValue(state, out var menuState))
            {
                SwitchState(menuState);
            }
        }
        public void SwitchState<T>(T state) where T : IState<MenuController>
        {
            CurrentState?.Exit();
            state?.Enter();
        }
    }
}
