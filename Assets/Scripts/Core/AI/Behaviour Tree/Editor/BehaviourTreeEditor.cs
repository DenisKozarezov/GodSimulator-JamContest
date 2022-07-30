using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.Callbacks;

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
        [OnOpenAsset]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            if (Selection.activeObject is BehaviourTree)
            {
                OpenWindow();
                return true;
            }
            return false;
        }
        private void OnEnable()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }
        private void OnDisable()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
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
            _behaviourView.OnNodeSelected += OnNodeSelectionChanged;
            OnSelectionChange();
        }
        private void OnSelectionChange()
        {
            BehaviourTree tree = null;

            // Runtime Game Objects clicking
            if (EditorApplication.isPlaying && Selection.activeGameObject)
            {
                IBehaviourTreeRunner runner = Selection.activeGameObject.GetComponent<IBehaviourTreeRunner>();
                if (runner != null) tree = runner.BehaviourTree;
            }

            // Editor Scriptable Objects clicking
            BehaviourTree SO = Selection.activeObject as BehaviourTree;
            if (SO && AssetDatabase.CanOpenAssetInEditor(SO.GetInstanceID()))
            {
                tree = SO;
            }

            // Open tree in editor. Disable it in runtime if needed
            if (tree)
            {
                _behaviourView?.PopulateView(tree);
                if (EditorApplication.isPlaying) _behaviourView?.SetEnabled(true & _behaviourView.EnableRuntimeEdit);
            }
        }
        private void OnInspectorUpdate()
        {
            _behaviourView?.UpdateNodesState();
        }
        private void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            switch (state)
            {
                case PlayModeStateChange.EnteredEditMode: case PlayModeStateChange.EnteredPlayMode:                 
                    OnSelectionChange();
                    break;
                case PlayModeStateChange.ExitingPlayMode:
                    _behaviourView?.SetEnabled(true);
                    break;
            }
        }
        private void OnNodeSelectionChanged(NodeView nodeView)
        {
            _inspectorView?.UpdateSelection(nodeView);
        }
    }
}