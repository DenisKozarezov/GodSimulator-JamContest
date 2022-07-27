using System.Collections.Generic;

namespace Core.AI.BehaviourTree.Nodes.Composites
{
    public abstract class CompositeNode : Node
    {
        protected List<Node> Children = new List<Node>();

        protected override void OnStart() { }
        protected override void OnStop() { }
        protected override abstract NodeState OnUpdate();
        public override IEnumerable<Node> GetChildren()
        {
            return Children;
        }
        public override void AddChild(Node node)
        {
            if (!Children.Contains(node)) Children.Add(node);
        }
        public override void RemoveChild(Node node)
        {
            Children.Remove(node);
        }
        public override Node Clone()
        {
            CompositeNode clone = Instantiate(this);
            clone.Children = this.Children.ConvertAll(c => c.Clone());
            return clone;
        }
    }
}
