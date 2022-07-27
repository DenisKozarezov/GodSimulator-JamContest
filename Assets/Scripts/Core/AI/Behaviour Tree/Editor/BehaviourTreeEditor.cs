using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Core.AI.BehaviourTree.Editor
{
    internal class BehaviourTreeEditor : EditorWindow
    {
        private BehaviourTreeView _behaviourView;
        private InspectorView _inspectorView;

        [MenuItem("Behaviour Tree/Editor")]
        public static void OpenWindow()
        {
            BehaviourTreeEditor wnd = GetWindow<BehaviourTreeEditor>();
            wnd.titleContent = new GUIContent(Constants.WindowTitle);
        }

        private void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            // Import UXML
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(Constants.UXMLPath);
            visualTree.CloneTree(root);

            // A stylesheet can be added to a VisualElement.
            // The style will be applied to the VisualElement and all of its children.
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(Constants.USSPath);
            root.styleSheets.Add(styleSheet);

            _behaviourView = root.Q<BehaviourTreeView>();
            _inspectorView = root.Q<InspectorView>();

            OnSelectionChange();
        }
        private void OnSelectionChange()
        {
            BehaviourTree tree = Selection.activeObject as BehaviourTree;
            if (tree)
            {
                _behaviourView.PopulateView(tree);
            }
        }
    }
}