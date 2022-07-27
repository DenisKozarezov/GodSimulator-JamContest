using Core.AI.BehaviourTree.Nodes.Decorators;

namespace Core.AI.BehaviourTree.Nodes
{
    public class RootNode : DecoratorNode
    {
        protected override NodeState OnUpdate()
        {
            if (Child == null) 
                return NodeState.Running;
            else 
                return Child.Update();
        }
    }
}
