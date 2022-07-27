using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using Node = Core.AI.BehaviourTree.Nodes.Node;

namespace Core.AI.BehaviourTree.Editor
{
    internal class BehaviourTreeView : GraphView
    {
        private BehaviourTree _tree;
        internal event Action<NodeView> OnNodeSelected;

        public new class UxmlFactory : UxmlFactory<BehaviourTreeView, GraphView.UxmlTraits> { }
        
        public BehaviourTreeView()
        {
            Insert(0, new GridBackground());
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(Constants.USSPath);
            styleSheets.Add(styleSheet);
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            var types = TypeCache.GetTypesDerivedFrom<Node>();
            foreach (var type in types)
            {
                evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", (action) => CreateNode(type));
            }
        }
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.Where(endPort =>
                endPort.direction != startPort.direction &&
                endPort.node != startPort.node
            ).ToList();
        }
        internal void PopulateView(BehaviourTree tree)
        {
            _tree = tree;
            graphViewChanged -= OnGraphViewChanged;
            DeleteElements(graphElements);
            graphViewChanged += OnGraphViewChanged;

            if (_tree.RootNode == null)
            {
                _tree.RootNode = _tree.CreateNode(typeof(Nodes.RootNode));
                SaveTree(_tree);
            }

            // Create Node Views
            tree.Nodes.ForEach(node => CreateNodeView(node));

            // Create Node Edges
            tree.Nodes.ForEach(node => CreateNodeEdge(node));
        }
        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            if (graphViewChange.elementsToRemove != null)
            {
                OnElementsToRemove(ref graphViewChange);
            }

            if (graphViewChange.edgesToCreate != null)
            {
                OnEdgesToCreate(ref graphViewChange);
            }
            return graphViewChange;
        }
        private void OnElementsToRemove(ref GraphViewChange graphViewChange)
        {
            graphViewChange.elementsToRemove.ForEach(element =>
            {
                NodeView view = element as NodeView;
                if (view != null)
                {
                    _tree.DeleteNode(view.Node);
                }

                Edge edge = element as Edge;
                if (edge != null)
                {
                    NodeView parentView = edge.output.node as NodeView;
                    NodeView childView = edge.input.node as NodeView;
                    parentView.Node.RemoveChild(childView.Node);
                }
            });
        }
        private GraphViewChange OnEdgesToCreate(ref GraphViewChange graphViewChange)
        {
            graphViewChange.edgesToCreate.ForEach(edge =>
            {
                NodeView parentView = edge.output.node as NodeView;
                NodeView childView = edge.input.node as NodeView;
                parentView.Node.AddChild(childView.Node);
            });
            return graphViewChange;
        }

        private NodeView FindNodeView(Node node)
        {
            return GetNodeByGuid(node.Guid) as NodeView;
        }
        private void CreateNode(Type type)
        {
            Node node = _tree.CreateNode(type);
            CreateNodeView(node);
        }
        private void CreateNodeView(Node node)
        {
            if (node == null) return;

            NodeView nodeView = new NodeView(node);
            nodeView.OnNodeSelected += OnNodeSelected;
            AddElement(nodeView);
        }
        private void CreateNodeEdge(Node node)
        {
            IEnumerable<Node> children = node.GetChildren();
            if (node == null || children == null || children.Count() == 0) return;

            NodeView parentView = FindNodeView(node);
            foreach (Node child in children)
            {
                NodeView childView = FindNodeView(child);
                Edge edge = parentView.OutputPort.ConnectTo(childView.InputPort);
                AddElement(edge);
            }
        }
        private void SaveTree(BehaviourTree tree)
        {
            EditorUtility.SetDirty(tree);
            AssetDatabase.SaveAssets();
        }
    }
}
