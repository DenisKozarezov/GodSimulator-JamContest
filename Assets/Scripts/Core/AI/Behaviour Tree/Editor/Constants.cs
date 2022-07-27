namespace Core.AI.BehaviourTree.Editor
{
    internal static class Constants
    {
        private const string AssemblyPath = "Assets\\Scripts\\Core\\AI\\Behaviour Tree\\Editor\\";
        public const string WindowTitle = "Behaviour Tree Editor";
        public static string UXMLPath => string.Concat(AssemblyPath, "BehaviourTreeEditor.uxml");
        public static string USSPath => string.Concat(AssemblyPath, "BehaviourTreeEditor.uss");
    }
}