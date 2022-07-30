using UnityEngine.UIElements;

namespace Core.AI.BehaviourTree.Editor
{
    internal class InspectorView : VisualElement
    {
        private UnityEditor.Editor _editor;

        public new class UxmlFactory : UxmlFactory<InspectorView, InspectorView.UxmlTraits> { }

        internal void UpdateSelection(NodeView nodeView)
        {
            Clear();

            UnityEngine.Object.DestroyImmediate(_editor);
            _editor = UnityEditor.Editor.CreateEditor(nodeView.Node);
            IMGUIContainer container = new IMGUIContainer(() =>
            {
                if (_editor.target) _editor?.OnInspectorGUI();
            });
            Add(container);
        }
    }
}
