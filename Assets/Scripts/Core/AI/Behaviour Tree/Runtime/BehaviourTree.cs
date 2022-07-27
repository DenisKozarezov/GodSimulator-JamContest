using System;
using System.Collections.Generic;
using UnityEngine;
using Core.AI.BehaviourTree.Nodes;
using NodeState = Core.AI.BehaviourTree.Nodes.Node.NodeState;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Core.AI.BehaviourTree
{
    [CreateAssetMenu(menuName = "Configuration/AI/Create Behaviour Tree")]
    public class BehaviourTree : ScriptableObject
    {
        private NodeState _currentState = NodeState.Running;

        [HideInInspector]
        public Node RootNode;
        [HideInInspector]
        public List<Node> Nodes = new List<Node>();

        internal NodeState Update()
        {
            if (RootNode.State == NodeState.Running)
            {
                _currentState = RootNode.Update();
            }
            return _currentState;
        }        
        public BehaviourTree Clone()
        {
            BehaviourTree clone = Instantiate(this);
            clone.RootNode = clone.RootNode.Clone();
            return clone;
        }

#if UNITY_EDITOR
        private void AddNodeToTree(Node node)
        {
            AssetDatabase.AddObjectToAsset(node, this);
            AssetDatabase.SaveAssets();
        }
        private void RemoveNodeFromTree(Node node)
        {
            AssetDatabase.RemoveObjectFromAsset(node);
            AssetDatabase.SaveAssets();
        }
        public Node CreateNode(Type type)
        {
            Node node = ScriptableObject.CreateInstance(type) as Node;
            node.Name = type.Name;
            node.Guid = GUID.Generate().ToString();
            Nodes.Add(node);
            AddNodeToTree(node);
            return node;
        }
        public void DeleteNode(Node node)
        {
            Nodes.Remove(node);
            RemoveNodeFromTree(node);
            
            if (node is RootNode) RootNode = null;
        }
#endif
    }
}