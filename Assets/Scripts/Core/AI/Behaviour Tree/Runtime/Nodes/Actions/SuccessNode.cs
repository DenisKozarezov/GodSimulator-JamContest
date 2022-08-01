namespace Core.AI.BehaviourTree.Nodes.Actions
{
    internal class SuccessNode : ActionNode
    {
        protected override NodeState OnUpdate()
        {
            return NodeState.Success;
        }
    }
}
