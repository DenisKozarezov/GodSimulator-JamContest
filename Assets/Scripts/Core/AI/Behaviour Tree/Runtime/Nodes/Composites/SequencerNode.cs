namespace Core.AI.BehaviourTree.Nodes.Composites
{
    public class SequencerNode : CompositeNode
    {
        private int _currentIndex;
        private bool AllChildrenEnumerated => _currentIndex == Children.Count;

        protected override void OnStart()
        {
            _currentIndex = 0;
        }
        protected override NodeState OnUpdate()
        {
            Node child = Children[_currentIndex];
            switch (child.State)
            {
                case NodeState.Success: _currentIndex++; break;
                default: return child.State;
            }
            return AllChildrenEnumerated ? NodeState.Success : NodeState.Running;
        }
    }
}