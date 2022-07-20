namespace Core.Cities
{
    public interface ICityStrategy
    {
        CityScript City { get; }
        bool Interactable { set; get; }
    }
}