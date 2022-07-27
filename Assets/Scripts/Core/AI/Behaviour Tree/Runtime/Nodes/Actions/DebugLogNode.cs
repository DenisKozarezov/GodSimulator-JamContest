using UnityEngine;

namespace Core.AI.BehaviourTree.Nodes.Actions
{
    internal class DebugLogNode : ActionNode
    {
        [SerializeField]
        private string _message;

        protected override NodeState OnUpdate()
        {
            Debug.Log(_message);
            return NodeState.Success;
        }
    }
}