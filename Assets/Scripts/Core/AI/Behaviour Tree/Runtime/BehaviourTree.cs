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

        private void Traverse(Node node, Action<Node> action)
        {
            if (node != null)
            {
                action?.Invoke(node);
                foreach (Node child in node.GetChildren())
                {
                    Traverse(child, action);
                }
            }
        }
        public NodeState Update()
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
        public void BindAgent(AIBehaviourAgent agent)
        {
            Traverse(RootNode, (node) => node.Agent = agent);
        }
        public BehaviourTree Clone()
        {
            BehaviourTree clone = Instantiate(this);
            clone.RootNode = clone.RootNode.Clone();
            clone.Nodes = new List<Node>();
            Traverse(clone.RootNode, (node) =>
            {
                clone.Nodes.Add(node);
            });
            return clone;
        }

#if UNITY_EDITOR
        [Header("Tree Settings")]
        [SerializeField]
        private Orientation _treeOrientation;
        [SerializeField]
        private bool _enableRuntimeEdit;
        public Orientation TreeOrientation => _treeOrientation;
        public bool EnableRuntimeEdit => _enableRuntimeEdit;

        public static string ParseTypeToName(Type type)
        {
            string name = type.Name.Substring(0, type.Name.IndexOf("Node"));
            int length = name.Length;
            int i = 0;
            while (i < length - 1)
            {
                if (char.IsUpper(name[i + 1]))
                {
                    name = name.Insert(i + 1, " ");
                    i++;
                }
                i++;
            }
            return name;
        }
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
            node.Name = ParseTypeToName(type);
            node.Guid = GUID.Generate().ToString();

            Undo.RecordObject(this, "Create Node (Behaviour Tree)");
            Nodes.Add(node);
            Undo.RegisterCreatedObjectUndo(node, "Create Node (Behaviour Tree)");
            
            AddNodeToTree(node);
            return node;
        }
        public void RemoveNode(Node node)
        {
            Undo.RecordObject(this, "Remove Node (Behaviour Tree)");
            Nodes.Remove(node);
            RemoveNodeFromTree(node);
            
            if (node is RootNode) RootNode = null;
        }
#endif
    }
}