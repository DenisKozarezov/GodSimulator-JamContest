using UnityEngine;

namespace Core.AI.BehaviourTree.Nodes.Actions
{
    internal class WaitNode : ActionNode
    {
        [SerializeField, Min(0f)]
        private float _duration;
        private float _startTime;

        protected override void OnStart()
        {
            _startTime = Time.time;
        }
        protected override NodeState OnUpdate()
        {
            if (Time.time - _startTime > _duration)
            { 
                return NodeState.Success;
            }
            return NodeState.Running;
        }
    }
}
