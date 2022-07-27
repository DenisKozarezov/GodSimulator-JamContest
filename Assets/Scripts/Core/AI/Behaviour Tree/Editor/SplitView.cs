using UnityEngine.UIElements;

namespace Core.AI.BehaviourTree.Editor
{
    internal class SplitView : TwoPaneSplitView
    {
        public new class UxmlFactory : UxmlFactory<SplitView, TwoPaneSplitView.UxmlTraits> { }
    }
}
