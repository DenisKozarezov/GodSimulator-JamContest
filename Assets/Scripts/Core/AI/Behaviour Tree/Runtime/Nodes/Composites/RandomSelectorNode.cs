using System;

namespace Core.AI.BehaviourTree.Nodes.Composites
{
    internal class RandomSelectorNode : CompositeNode
    {
        private Unity.Mathematics.Random _random;
        protected override void OnStart()
        {
            _random = new Unity.Mathematics.Random();
            _random.InitState(unchecked((uint)DateTime.Now.Ticks));
        }
        protected override NodeState OnUpdate()
        {
            int rand = _random.NextInt(0, Children.Count);
            return Children[rand].Update();
        }
    }
}