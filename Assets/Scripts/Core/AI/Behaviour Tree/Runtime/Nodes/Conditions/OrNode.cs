namespace Core.AI.BehaviourTree.Nodes.Conditions
{
    internal class OrNode : ConditionNode
    {
        [Input] bool A;
        [Input] bool B;
        [Output] bool Result;
        protected override bool Condition()
        {
            return A | B;
        }
        protected override NodeState OnUpdate()
        {
            return Condition() ? NodeState.Success : NodeState.Running;
        }
    }
}
