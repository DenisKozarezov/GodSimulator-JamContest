﻿using System.Collections.Generic;
using UnityEngine;

namespace Core.AI.BehaviourTree.Nodes.Conditions
{
    public abstract class ConditionNode : Node
    {
        [SerializeField, HideInInspector] protected Node Child;

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
            if (Child == null) yield break;
            yield return Child;
        }
        public sealed override void AddChild(Node node)
        {
            Child = node;
        }
        public sealed override void RemoveChild(Node node)
        {
            Child = null;
        }
#endif
    }
}