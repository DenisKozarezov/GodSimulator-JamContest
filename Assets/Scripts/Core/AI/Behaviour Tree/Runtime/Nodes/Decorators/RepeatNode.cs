namespace Core.AI.BehaviourTree.Nodes.Decorators
{
    public class RepeatNode : DecoratorNode
    {
        protected override NodeState OnUpdate()
        {
            Child.Update();
            return NodeState.Running;
        }
    }
}