namespace Core.AI.BehaviourTree.Nodes.Conditions
{
    internal class WaitUntilNode : ConditionNode
    {
        protected override NodeState OnUpdate()
        {
            return Condition() ? NodeState.Success : NodeState.Running;
        }
    }
}
