using UnityEngine;
using Core.AI.BehaviourTree.Nodes;

namespace Core.AI.BehaviourTree.Editor
{
    internal class NodeView : UnityEditor.Experimental.GraphView.Node
    {
        internal readonly Node Node;

        internal NodeView(Node node)
        {
            Node = node;
            title = node.Name;
            viewDataKey = node.Guid;
            style.left = node.Position.x;
            style.top = node.Position.y;
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            Node.Position.x = newPos.xMin;
            Node.Position.y = newPos.yMin;
        }
    }
}
