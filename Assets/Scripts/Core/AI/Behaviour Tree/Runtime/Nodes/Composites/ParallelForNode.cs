namespace Core.AI.BehaviourTree.Nodes.Composites
{
    internal class ParallelForNode : CompositeNode
    {
        private int _successCount;

        protected override void OnStart()
        {
            _successCount = 0;
        }
        protected override NodeState OnUpdate()
        {
            for (int i = 0; i < Children.Count; i++)
            {
                switch (Children[i].Update())
                {
                    case NodeState.Failure: return NodeState.Failure;
                    case NodeState.Success: _successCount++; break;
                }
                CurrentIndex = i;
            }
            return _successCount == Children.Count ? NodeState.Success : NodeState.Running;
        }
    }
}