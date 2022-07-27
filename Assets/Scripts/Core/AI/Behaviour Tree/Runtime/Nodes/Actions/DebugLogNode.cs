using UnityEngine;

namespace Core.AI.BehaviourTree.Nodes.Actions
{
    internal class DebugLogNode : ActionNode
    {
        public string Message;

        protected override NodeState OnUpdate()
        {
            Debug.Log(Message);
            return NodeState.Success;
        }
    }
}