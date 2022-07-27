using UnityEngine;

namespace Core.AI.BehaviourTree
{
    internal class BehaviourTreeRunner : MonoBehaviour
    {
        private BehaviourTree _tree;

        private void Start()
        {
            _tree = ScriptableObject.CreateInstance<BehaviourTree>();

           
        }
        private void Update()
        {
            _tree.Update();
        }
    }
}