using Core.Cities;
using Core.Models;

namespace Core.Infrastructure
{
    public struct TempleDragBeginSignal
    {
        public TempleStrategy Temple;
    }
    public struct TempleDragEndSignal
    {
        public GodModel God;
        public TempleStrategy Temple;
        public CityScript Target;
    }
}