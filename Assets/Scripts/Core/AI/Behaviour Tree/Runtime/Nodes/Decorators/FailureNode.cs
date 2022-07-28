namespace Core.AI.BehaviourTree.Nodes.Decorators
{
    internal class FailureNode : DecoratorNode
    {
        protected override NodeState OnUpdate()
        {
            return NodeState.Failure;
        }
    }
}
