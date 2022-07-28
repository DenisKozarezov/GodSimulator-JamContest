using Core.AI.BehaviourTree;
using UnityEngine;
using Tree = Core.AI.BehaviourTree.BehaviourTree;

namespace Core.AI
{
    public class AIController : MonoBehaviour, IBehaviourTreeRunner
    {
        [SerializeField]
        private Tree _behaviourTree;
        public Tree BehaviourTree => _behaviourTree;

        private void Awake()
        {
            _behaviourTree = _behaviourTree.Clone();
            _behaviourTree.BindAgent(new BotComponent());
        }
        private void Start()
        {
            _behaviourTree.Simulate();
        }
        private void Update()
        {
            _behaviourTree?.Update();
        }
    }
}