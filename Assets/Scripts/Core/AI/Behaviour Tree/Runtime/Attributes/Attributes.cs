using System;
using UnityEngine;

namespace Core.AI.BehaviourTree
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class NodeWidthAttribute : Attribute
    {
        public readonly int Width;

        public NodeWidthAttribute(int width)
        {
            Width = width;
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class NodeTintAttribute : Attribute
    {
        public readonly Color Color;

        public NodeTintAttribute(float r, float g, float b)
        {
            Color = new Color(r, g, b);
        }

        public NodeTintAttribute(string hex)
        {
            ColorUtility.TryParseHtmlString(hex, out Color);
        }

        public NodeTintAttribute(byte r, byte g, byte b)
        {
            Color = new Color32(r, g, b, byte.MaxValue);
        }
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class InputAttribute : Attribute
    {
        public InputAttribute()
        {

        }
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class OutputAttribute : Attribute
    {
        public OutputAttribute()
        {

        }
    }
}