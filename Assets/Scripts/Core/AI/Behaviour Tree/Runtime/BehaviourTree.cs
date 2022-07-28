using System;
using System.Collections.Generic;
using UnityEngine;
using Core.AI.BehaviourTree.Nodes;
using Node = Core.AI.BehaviourTree.Nodes.Node;
using NodeState = Core.AI.BehaviourTree.Nodes.Node.NodeState;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Experimental.GraphView;
#endif

namespace Core.AI.BehaviourTree
{
    [Serializable]
    [CreateAssetMenu(menuName = "Configuration/AI/Create Behaviour Tree")]
    public class BehaviourTree : ScriptableObject
    {
        private bool _simulate;
        private NodeState _currentState = NodeState.Running;

        [SerializeField, HideInInspector]
        public Node RootNode;
        [SerializeField, HideInInspector]
        public List<Node> Nodes = new List<Node>();

        internal NodeState Update()
        {
            if (!_simulate) return NodeState.Success;

            if (RootNode.State == NodeState.Running)
            {
                _currentState = RootNode.Update();
            }
            return _currentState;
        }        
        public void Simulate()
        {
            _simulate = true;
        }
        public void Stop()
        {
            _simulate = false;
        }
        public BehaviourTree Clone()
        {
            BehaviourTree clone = Instantiate(this);
            clone.RootNode = clone.RootNode.Clone();
            return clone;
        }

#if UNITY_EDITOR
        [Header("Tree Settings")]
        [SerializeField]
        private Orientation _treeOrientation;
        public Orientation TreeOrientation => _treeOrientation;

        private void AddNodeToTree(Node node)
        {
            AssetDatabase.AddObjectToAsset(node, this);
            AssetDatabase.SaveAssets();
        }
        private void RemoveNodeFromTree(Node node)
        {
            Undo.DestroyObjectImmediate(node);
            AssetDatabase.SaveAssets();
        }
        public Node CreateNode(Type type)
        {
            Node node = ScriptableObject.CreateInstance(type) as Node;
            node.Name = type.Name;
            node.Guid = GUID.Generate().ToString();

            Undo.RecordObject(this, "Create Node (Behaviour Tree)");
            Nodes.Add(node);
            Undo.RegisterCreatedObjectUndo(node, "Create Node (Behaviour Tree)");
            
            AddNodeToTree(node);
            return node;
        }
        public void DeleteNode(Node node)
        {
            Undo.RecordObject(this, "Remove Node (Behaviour Tree)");
            Nodes.Remove(node);
            RemoveNodeFromTree(node);
            
            if (node is RootNode) RootNode = null;
        }
#endif
    }
}