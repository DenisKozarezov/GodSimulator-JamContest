namespace Core.AI.BehaviourTree.Nodes.Composites
{
    internal class SequencerNode : CompositeNode
    {
        protected override NodeState OnUpdate()
        {
            if (Children.Count == 0) return NodeState.Failure;

            Node child = Children[CurrentIndex];
            NodeState state = child.Update();
            switch (state)
            {
                case NodeState.Success: CurrentIndex++; break;
                default: return state;
            }
            return AllChildrenEnumerated ? NodeState.Success : NodeState.Running;
        }
    }
}