namespace Core.AI.BehaviourTree
{
    public enum BehaviourPattern : byte
    {
        Standard = 0x00,
        Aggressive = 0x01,
        Passive = 0x02,
        Fastest = 0x04,
    }

    public interface AIBehaviourAgent
    {
        BehaviourPattern BehaviourPattern { get; }
    }
}