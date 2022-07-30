using System;
using Zenject;

namespace Core.Cities
{
    public interface ICityStrategy : IEquatable<ICityStrategy>
    {
        CityScript City { get; }
        bool Interactable { set; get; }
        void Disable();
    }

    public class CityStrategyFactory : PlaceholderFactory<TempleStrategy> { }
}