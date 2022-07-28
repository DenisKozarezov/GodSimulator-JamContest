namespace Core.AI.BehaviourTree.Nodes.Composites
{
    internal class ParallelForNode : CompositeNode
    {
        protected override NodeState OnUpdate()
        {
            for (int i = 0; i < Children.Count; i++)
            {
                Node child = Children[i];

                NodeState state = child.Update();
                if (state == NodeState.Failure) return NodeState.Failure;
                CurrentIndex = i;
            }
            return NodeState.Success;
        }
    }
}