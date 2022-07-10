using System;
using UnityEngine;

namespace Core.Input
{
    public interface IInputSystem
    {
        Action Escape { get; }
        Vector2 MousePosition { get; }
    }
}