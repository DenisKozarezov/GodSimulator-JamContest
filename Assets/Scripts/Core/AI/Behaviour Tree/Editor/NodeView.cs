using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using Core.AI.BehaviourTree.Nodes.Actions;
using Core.AI.BehaviourTree.Nodes.Composites;
using Node = Core.AI.BehaviourTree.Nodes.Node;

namespace Core.AI.BehaviourTree.Editor
{
    internal class NodeView : UnityEditor.Experimental.GraphView.Node
    {
        internal readonly Node Node;
        private readonly Orientation _orientation;
        private Port _inputPort;
        private Port _outputPort;
        internal Port InputPort => _inputPort;
        internal Port OutputPort => _outputPort;

        internal event Action<NodeView> OnNodeSelected;

        internal NodeView(Node node, Orientation orientation)
        {
            Node = node;
            _orientation = orientation;
            title = node.Name;
            viewDataKey = node.Guid;
            style.left = node.Position.x;
            style.top = node.Position.y;

            CreateInputPorts();
            CreateOutputPorts();
        }

        private void CreateInputPorts()
        {
            if (Node is not Nodes.RootNode)
            {
                _inputPort = InstantiatePort(_orientation, Direction.Input, Port.Capacity.Single, typeof(bool));
            }
            
            if (_inputPort != null)
            {
                _inputPort.portName = Constants.InputPort;
                inputContainer.Add(_inputPort);
            }
        }
        private void CreateOutputPorts()
        {
            Port.Capacity capacity = Port.Capacity.Single;
            if (Node is CompositeNode) 
                capacity = Port.Capacity.Multi;

            // Action Nodes don't have children (output ports)
            if (Node is not ActionNode)
                _outputPort = InstantiatePort(_orientation, Direction.Output, capacity, typeof(bool));

            if (_outputPort != null)
            {
                _outputPort.portName = Constants.OutputPort;
                outputContainer.Add(_outputPort);
            }
        }
        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            Undo.RecordObject(Node, "Move Node (Behaviour Tree)");
            Node.Position.x = newPos.xMin;
            Node.Position.y = newPos.yMin;
            EditorUtility.SetDirty(Node);
        }
        public override void OnSelected()
        {
            base.OnSelected();
            OnNodeSelected?.Invoke(this);
        }
    }
}
