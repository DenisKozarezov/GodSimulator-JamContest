using Core.AI.BehaviourTree;

namespace Core.AI
{
    public enum BehaviourPattern : byte
    {
        Standard = 0x00,
        Aggressive = 0x01,
        Passive = 0x02,
        Fastest = 0x04,
    }

    public class AIBehaviourAgent
    {
        private BehaviourPattern _behaviourPattern;

        public AIBehaviourAgent()
        {

        }
    }
}