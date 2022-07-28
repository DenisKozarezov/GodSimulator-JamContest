using UnityEngine;

namespace Core.AI.BehaviourTree
{
    public class BehaviourTreeRunner : MonoBehaviour
    {
        [SerializeField]
        private BehaviourTree _behaviourTree;
        public BehaviourTree BehaviourTree => _behaviourTree;

        private void Awake()
        {
            _behaviourTree = _behaviourTree.Clone();           
        }
        private void Start()
        {
            _behaviourTree.Simulate();
        }
        private void Update()
        {
            _behaviourTree.Update();
        }
    }
}