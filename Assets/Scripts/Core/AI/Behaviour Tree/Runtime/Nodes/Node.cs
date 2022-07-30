using System.Collections.Generic;
using UnityEngine;

namespace Core.AI.BehaviourTree.Nodes
{
    public abstract class Node : ScriptableObject
    {
        public enum NodeState : byte
        {
            Running = 0x00,
            Failure = 0x01,
            Success = 0x02
        }

        private bool _started;
        private NodeState _state = NodeState.Running;
        public NodeState State => _state;

        public NodeState Update()
        {
            if (!_started)
            {
                OnStart();
                _started = true;
            }
            _state = OnUpdate();

            // If Node returned some result (whether success or failure), then stop it
            if (_state == NodeState.Failure || _state == NodeState.Success)
            {
                OnStop();
                _started = false;
            }
            return _state;
        }
        protected abstract void OnStart();
        protected abstract void OnStop();
        protected abstract NodeState OnUpdate();
        public abstract IEnumerable<Node> GetChildren();
    }
}