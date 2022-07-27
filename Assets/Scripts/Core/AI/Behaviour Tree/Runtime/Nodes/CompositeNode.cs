using System.Collections.Generic;

namespace Core.AI.BehaviourTree.Nodes.Composites
{
    internal abstract class CompositeNode : Node
    {
        protected List<Node> Children = new List<Node>();

        protected override void OnStart()
        {

        }
        protected override void OnStop()
        {

        }
        protected override abstract NodeState OnUpdate();
        public override IEnumerable<Node> GetChildren()
        {
            return Children;
        }
    }
}
