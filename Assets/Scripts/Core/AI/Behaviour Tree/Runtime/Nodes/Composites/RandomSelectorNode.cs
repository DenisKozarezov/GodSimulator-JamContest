using System;

namespace Core.AI.BehaviourTree.Nodes.Composites
{
    internal class RandomSelectorNode : CompositeNode
    {
        protected override NodeState OnUpdate()
        {
            var random = new Unity.Mathematics.Random();
            random.InitState(unchecked((uint)DateTime.Now.Ticks));
            int rand = random.NextInt(0, Children.Count - 1);
            return Children[rand].Update();
        }
    }
}