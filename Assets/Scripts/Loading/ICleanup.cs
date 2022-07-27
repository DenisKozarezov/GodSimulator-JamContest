namespace Core.Loading
{
    public interface ICleanup
    {
        string SceneName { get; }
        void Cleanup();
    }
}
