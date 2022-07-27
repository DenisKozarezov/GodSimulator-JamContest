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
    [CreateAssetMenu()]
    public class BehaviourTree : ScriptableObject
    {
        private Node _rootNode;
        private NodeState _currentState = NodeState.Running;

        [HideInInspector]
        public List<Node> Nodes = new List<Node>();

        internal NodeState Update()
        {
            if (_rootNode.State == NodeState.Running)
            {
                _currentState = _rootNode.Update();
            }
            return _currentState;
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
        }
#endif
    }
}