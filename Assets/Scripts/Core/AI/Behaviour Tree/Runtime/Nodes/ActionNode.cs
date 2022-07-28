using System.Collections.Generic;
using System.Linq;

namespace Core.AI.BehaviourTree.Nodes.Actions
{
    public abstract class ActionNode : Node
    {
        protected override void OnStart() { }
        protected override void OnStop() { }
        protected override abstract NodeState OnUpdate();

#if UNITY_EDITOR
        public sealed override IEnumerable<Node> GetChildren()
        {
            return Enumerable.Empty<Node>();
        }
        public sealed override void AddChild(Node node) { }
        public sealed override void RemoveChild(Node node) { }
#endif
    }
}