using Core.AI.BehaviourTree.Nodes;
using UnityEngine;

namespace Core.AI.BehaviourTree
{
    public class BehaviourTreeRunner : MonoBehaviour
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