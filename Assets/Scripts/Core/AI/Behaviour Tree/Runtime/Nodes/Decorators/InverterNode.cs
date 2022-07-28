namespace Core.AI.BehaviourTree.Nodes.Decorators
{
    internal class InverterNode : DecoratorNode
    {
        protected override NodeState OnUpdate()
        {
            return State == NodeState.Success ? NodeState.Failure : NodeState.Success;
        }
    }
}
