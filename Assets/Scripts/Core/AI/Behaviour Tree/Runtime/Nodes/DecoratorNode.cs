using System.Collections.Generic;

namespace Core.AI.BehaviourTree.Nodes.Decorators
{
    internal abstract class DecoratorNode : Node
    {
        protected Node Child;

        protected override void OnStart()
        {
           
        }
        protected override void OnStop()
        {
         
        }
        protected override abstract NodeState OnUpdate();
        public override IEnumerable<Node> GetChildren()
        {
            yield return Child;
        }
    }
}