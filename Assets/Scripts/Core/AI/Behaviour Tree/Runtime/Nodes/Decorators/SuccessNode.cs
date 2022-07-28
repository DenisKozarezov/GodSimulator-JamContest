namespace Core.AI.BehaviourTree.Nodes.Decorators
{
    internal class SuccessNode : DecoratorNode
    {
        protected override NodeState OnUpdate()
        {
            return NodeState.Success;
        }
    }
}
