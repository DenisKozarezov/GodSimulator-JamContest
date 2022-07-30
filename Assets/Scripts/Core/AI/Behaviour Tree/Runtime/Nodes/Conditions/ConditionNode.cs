using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.AI.BehaviourTree.Nodes.Conditions
{
    public abstract class ConditionNode : Node
    {
        protected override void OnStart() { }
        protected override void OnStop() { }
        protected override abstract NodeState OnUpdate();
        protected abstract bool Condition();
        public override Node Clone()
        {
            return Instantiate(this);
        }

#if UNITY_EDITOR
        public sealed override IEnumerable<Node> GetChildren()
        {
            return Enumerable.Empty<Node>();
        }
        public sealed override void AddChild(Node node)
        {

        }
        public sealed override void RemoveChild(Node node)
        {

        }
#endif
    }
}