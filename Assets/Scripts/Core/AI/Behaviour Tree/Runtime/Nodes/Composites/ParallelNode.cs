namespace Core.AI.BehaviourTree.Nodes.Composites
{
    internal class ParallelNode : CompositeNode
    {
        protected override NodeState OnUpdate()
        {
            return NodeState.Running;
        }
    }
}