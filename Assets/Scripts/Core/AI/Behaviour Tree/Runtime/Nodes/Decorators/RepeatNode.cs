namespace Core.AI.BehaviourTree.Nodes.Decorators
{
    internal class RepeatNode : DecoratorNode
    {
        protected override NodeState OnUpdate()
        {
            Child.Update();
            return NodeState.Running;
        }
    }
}