namespace Core.AI.BehaviourTree.Nodes.Composites
{
    internal class ParallelForNode : CompositeNode
    {
        protected override NodeState OnUpdate()
        {
            for (int i = 0; i < Children.Count; i++)
            {
                NodeState state = Children[i].Update();
                if (state == NodeState.Failure) return NodeState.Failure;
                CurrentIndex = i;
            }
            return NodeState.Success;
        }
    }
}