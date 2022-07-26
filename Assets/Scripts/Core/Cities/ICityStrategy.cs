using System;

namespace Core.Cities
{
    public interface ICityStrategy : IEquatable<ICityStrategy>
    {
        CityScript City { get; }
        bool Interactable { set; get; }
        void Disable();
    }
}