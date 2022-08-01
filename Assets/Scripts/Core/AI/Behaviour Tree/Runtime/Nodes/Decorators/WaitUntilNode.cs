namespace Core.AI.BehaviourTree.Nodes.Decorators
{
    internal class WaitUntilNode : DecoratorNode
    {
        [Input] Node Await;
        protected override NodeState OnUpdate()
        {
            switch (Await.State)
            {
                case NodeState.Success: return Child.Update();
                default: return Await.State;
            }
        }
    }
}
