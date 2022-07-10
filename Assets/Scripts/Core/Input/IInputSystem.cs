using System;
using UnityEngine;

namespace Core.Input
{
    public interface IInputSystem
    {
        Action Escape { set; get; }
        float MouseWheelDelta { get; }
        Vector2 MousePosition { get; }
    }
}