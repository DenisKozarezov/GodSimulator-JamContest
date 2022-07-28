using Core.AI.BehaviourTree;

namespace Core.AI
{
    public class BotComponent : AIBehaviourAgent
    {
        private BehaviourPattern _behaviourPattern;
        public BehaviourPattern BehaviourPattern => _behaviourPattern;

        public BotComponent()
        {
            
        }
    }
}