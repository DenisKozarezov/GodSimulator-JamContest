using Core.Infrastructure;
using UnityEngine;

namespace Core.UI
{
    public class UIContoller
    {
        private bool _selectionMode;

        public bool SelectionMode => _selectionMode;

        public void SetSelectionMode(SelectionModeChangedSignal selectionModeChangedSignal)
        {
            _selectionMode = selectionModeChangedSignal.Value;
        }
    }
}