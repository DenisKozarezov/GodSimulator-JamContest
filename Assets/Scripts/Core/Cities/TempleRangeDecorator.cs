namespace Core.Cities
{
    public abstract class TempleRangeDecorator
    {
        protected readonly TempleStrategy Temple;
        public TempleRangeDecorator(TempleStrategy temple) => Temple = temple;
        public abstract float GetRange();
    }

    public class TempleRangeVirtueLevelDecorator : TempleRangeDecorator
    {
        private float Coefficient = 1.1f;
        private byte virtueLevel = 1;
        public TempleRangeVirtueLevelDecorator(TempleStrategy temple) : base(temple) { }
        public override float GetRange()
        {
            return Temple.StartRange + (virtueLevel - 1) * Coefficient;
        }
    }
    public class TempleRangeSacrificeDecorator : TempleRangeDecorator
    {
        public TempleRangeSacrificeDecorator(TempleStrategy temple) : base(temple) { }
        public override float GetRange()
        {
            return Temple.StartRange;
        }
    }
}