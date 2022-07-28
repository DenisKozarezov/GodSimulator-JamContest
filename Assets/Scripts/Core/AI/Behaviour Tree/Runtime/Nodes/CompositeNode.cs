using System.Collections.Generic;
using UnityEngine;

namespace Core.AI.BehaviourTree.Nodes.Composites
{
    public abstract class CompositeNode : Node
    {
        protected int CurrentIndex;
        [SerializeField, HideInInspector]
        protected List<Node> Children = new List<Node>();

        protected bool AllChildrenEnumerated => CurrentIndex == Children.Count;

        protected override void OnStart() 
        {
            CurrentIndex = 0;
        }
        protected override void OnStop() { }
        protected override abstract NodeState OnUpdate();
        public override Node Clone()
        {
            CompositeNode clone = Instantiate(this);
            clone.Children = Children.ConvertAll(c => c.Clone());
            return clone;
        }

#if UNITY_EDITOR
        public sealed override IEnumerable<Node> GetChildren()
        {
            return Children;
        }
        public sealed override void AddChild(Node node)
        {
            if (node == null) return;
            if (!Children.Contains(node))
            {
                UnityEditor.Undo.RecordObject(this, "Add Child (Behaviour Tree)");
                Children.Add(node);
                UnityEditor.EditorUtility.SetDirty(this);
            }
        }
        public sealed override void RemoveChild(Node node)
        {
            if (node == null) return;
            UnityEditor.Undo.RecordObject(this, "Remove Child (Behaviour Tree)");
            Children.Remove(node);
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
    }
}
