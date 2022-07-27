using UnityEngine.UIElements;

namespace Core.AI.BehaviourTree.Editor
{
    internal class InspectorView : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<InspectorView, InspectorView.UxmlTraits> { }
        public InspectorView()
        {

        }
    }
}
