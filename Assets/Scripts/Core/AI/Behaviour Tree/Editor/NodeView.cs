using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using Core.AI.BehaviourTree.Nodes.Actions;
using Core.AI.BehaviourTree.Nodes.Conditions;
using Core.AI.BehaviourTree.Nodes.Composites;
using Core.AI.BehaviourTree.Nodes.Decorators;
using Core.AI.BehaviourTree.Nodes;
using Node = Core.AI.BehaviourTree.Nodes.Node;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEngine.UIElements;

namespace Core.AI.BehaviourTree.Editor
{
    internal class NodeView : UnityEditor.Experimental.GraphView.Node
    {
        internal readonly Node Node;
        private readonly Orientation _orientation;
        private Port _inputPort;
        private Port _outputPort;
        private LinkedList<Port> _dymamicInputPorts = new LinkedList<Port>();
        private LinkedList<Port> _dymamicOutputPorts = new LinkedList<Port>();
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

            SetupClasses();
            CreateInputPorts();
            CreateOutputPorts();
        }
        internal NodeView(Node node, Orientation orientation, string stylePath) : base(stylePath)
        {
            Node = node;
            _orientation = orientation;
            title = node.Name;
            viewDataKey = node.Guid;
            style.left = node.Position.x;
            style.top = node.Position.y;

            SetupClasses();
            CreateInputPorts();
            CreateOutputPorts();
        }

        private void SetupClasses()
        {
            if (Node is ActionNode) AddToClassList("action");
            if (Node is ConditionNode) AddToClassList("condition");
            if (Node is CompositeNode) AddToClassList("composite");
            if (Node is DecoratorNode)
            {
                if (Node is RootNode) AddToClassList("root");
                else AddToClassList("decorator");
            }
        }
        private void CreateInputPorts()
        {
            if (Node is not RootNode)
            {
                _inputPort = InstantiatePort(_orientation, Direction.Input, Port.Capacity.Single, typeof(bool));
                _inputPort.name = "port";
                _dymamicInputPorts = CreateDynamicInputPorts();             
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
            {
                _outputPort = InstantiatePort(_orientation, Direction.Output, capacity, typeof(bool));
                _outputPort.name = "port";
                _dymamicOutputPorts = CreateDynamicOutputPorts();
            }
            if (_outputPort != null)
            {
                _outputPort.portName = Constants.OutputPort;
                outputContainer.Add(_outputPort);
            }
        }
        private LinkedList<Port> CreateDynamicInputPorts()
        {
            var fields = GetBackingFields<InputAttribute>();
            foreach (var field in fields)
            {
                Port port = InstantiatePort(_orientation, Direction.Input, Port.Capacity.Single, field.FieldType);
                port.name = "port";
                port.portName = field.Name;
                _dymamicInputPorts.AddLast(port);
                inputContainer.Add(port);
            }
            return _dymamicInputPorts;
        }
        private LinkedList<Port> CreateDynamicOutputPorts()
        {
            var fields = GetBackingFields<OutputAttribute>();
            foreach (var field in fields)
            {
                Port port = InstantiatePort(_orientation, Direction.Output, Port.Capacity.Single, field.FieldType);
                port.name = "port";
                port.portName = field.Name;
                _dymamicOutputPorts.AddLast(port);
                outputContainer.Add(port);
            }
            return _dymamicOutputPorts;
        }
        private IEnumerable<FieldInfo> GetBackingFields<T>() where T : Attribute
        {
            return Node.GetType().GetFields().Where(field => Attribute.IsDefined(field, typeof(T)));
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
        internal void UpdateState()
        {
            if (Application.isPlaying)
            {
                Array values = Enum.GetValues(typeof(Node.NodeState));
                for (int i = 0; i < values.Length; i++)
                {
                    RemoveFromClassList(values.GetValue(i).ToString().ToLower());
                }

                string state = Node.State.ToString().ToLower();
                switch (Node.State)
                {
                    case Node.NodeState.Running:
                        if (Node.Started) AddToClassList(state);
                        break;
                    default: AddToClassList(state);
                        break;
                }
            }
        }
        internal void SortChildren()
        {
            if (Node as CompositeNode is var node)
            {
                node?.SortChildren(
                    _orientation == Orientation.Horizontal 
                    ? SortByVerticalPosition 
                    : SortByHorizontalPosition);
            }
        }

        private int SortByHorizontalPosition(Node left, Node right)
        {
            return left.Position.x < right.Position.x ? -1 : 1;
        }
        private int SortByVerticalPosition(Node left, Node right)
        {
            return left.Position.y < right.Position.y ? -1 : 1;
        }
    }
}
