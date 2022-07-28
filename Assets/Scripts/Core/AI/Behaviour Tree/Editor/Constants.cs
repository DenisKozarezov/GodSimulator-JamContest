namespace Core.AI.BehaviourTree.Editor
{
    internal static class Constants
    {
        private const string AssemblyPath = "Assets\\Scripts\\Core\\AI\\Behaviour Tree\\Editor\\";
        public const string WindowTitle = "Behaviour Tree Editor";
        public const string InputPort = "In Result";
        public const string OutputPort = "Out Result";
        public static string UXMLPath => string.Concat(AssemblyPath, "BehaviourTreeEditor.uxml");
        public static string USSPath => string.Concat(AssemblyPath, "BehaviourTreeEditor.uss");
        public static string NodeViewPath => string.Concat(AssemblyPath, "NodeView.uxml");
    }
}