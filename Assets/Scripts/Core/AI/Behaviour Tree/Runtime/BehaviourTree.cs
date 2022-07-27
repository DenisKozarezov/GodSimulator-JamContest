using UnityEngine;
using Core.AI.BehaviourTree.Nodes;
using NodeState = Core.AI.BehaviourTree.Nodes.Node.NodeState;

namespace Core.AI.BehaviourTree
{
    public class BehaviourTree : ScriptableObject
    {
        private Node _rootNode;
        private NodeState _currentState = NodeState.Running;

        public NodeState Update()
        {
            if (_rootNode.State == NodeState.Running)
            {
                _currentState = _rootNode.Update();
            }
            return _currentState;
        }
    }
}