namespace Core.AI.BehaviourTree.Nodes.Conditions
{
    internal class WaitUntilNode : ConditionNode
    {
        [Input] public int Hello;
        [Output] public int Hello1;

        protected override bool Condition()
        {
            return true;
        }
        protected override NodeState OnUpdate()
        {
            return Condition() ? NodeState.Success : NodeState.Running;
        }
    }
}
