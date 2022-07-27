using System.Collections.Generic;
using System.Linq;

namespace Core.AI.BehaviourTree.Nodes.Actions
{
    internal abstract class ActionNode : Node
    {
        protected override void OnStart() { }
        protected override void OnStop() { }
        protected override abstract NodeState OnUpdate();
        public override IEnumerable<Node> GetChildren()
        {
            return Enumerable.Empty<Node>();
        }
    }
}