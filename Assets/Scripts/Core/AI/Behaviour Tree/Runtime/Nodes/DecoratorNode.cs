using System.Collections.Generic;
using UnityEngine;

namespace Core.AI.BehaviourTree.Nodes.Decorators
{
    public abstract class DecoratorNode : Node
    {
        [SerializeField, HideInInspector]
        protected Node Child;

        protected override void OnStart() { }
        protected override void OnStop() { }
        protected override abstract NodeState OnUpdate();
        public override Node Clone()
        {
            DecoratorNode clone = Instantiate(this);
            clone.Child = Child.Clone();
            return clone;
        }

#if UNITY_EDITOR
        public sealed override IEnumerable<Node> GetChildren()
        {
            if (Child == null) yield break;
            yield return Child;
        }
        public sealed override void AddChild(Node node)
        {
            UnityEditor.Undo.RecordObject(this, "Add Child (Behaviour Tree)");
            Child = node;
            UnityEditor.EditorUtility.SetDirty(this);
        }
        public sealed override void RemoveChild(Node node)
        {
            UnityEditor.Undo.RecordObject(this, "Remove Child (Behaviour Tree)");
            Child = null;
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
    }
}