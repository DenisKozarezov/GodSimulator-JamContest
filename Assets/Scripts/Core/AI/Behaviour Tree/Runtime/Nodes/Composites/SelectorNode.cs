namespace Core.AI.BehaviourTree.Nodes.Composites
{
    internal class SelectorNode : CompositeNode
    {
        protected override NodeState OnUpdate()
        {
            for (var i = CurrentIndex; i < Children.Count; i++)
            {
                Node child = Children[CurrentIndex];

                NodeState state = child.Update();
                if (state != NodeState.Failure) return state;
                CurrentIndex++;
            }
            return NodeState.Failure;
        }
    }
}