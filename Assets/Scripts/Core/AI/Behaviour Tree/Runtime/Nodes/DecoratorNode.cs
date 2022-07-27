using System.Collections.Generic;

namespace Core.AI.BehaviourTree.Nodes.Decorators
{
    public abstract class DecoratorNode : Node
    {
        protected Node Child;

        protected override void OnStart() { }
        protected override void OnStop() { }
        protected override abstract NodeState OnUpdate();
        public override IEnumerable<Node> GetChildren()
        {
            if (Child == null) yield break;
            yield return Child;
        }
        public override void AddChild(Node node)
        {
            Child = node;
        }
        public override void RemoveChild(Node node)
        {
            Child = null;
        }
        public override Node Clone()
        {
            DecoratorNode clone = Instantiate(this);
            clone.Child = this.Child;
            return clone;
        }
    }
}