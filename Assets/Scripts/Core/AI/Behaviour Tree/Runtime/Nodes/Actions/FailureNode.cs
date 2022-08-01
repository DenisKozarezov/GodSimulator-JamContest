namespace Core.AI.BehaviourTree.Nodes.Actions
{
    internal class FailureNode : ActionNode
    {
        protected override NodeState OnUpdate()
        {
            return NodeState.Failure;
        }
    }
}
