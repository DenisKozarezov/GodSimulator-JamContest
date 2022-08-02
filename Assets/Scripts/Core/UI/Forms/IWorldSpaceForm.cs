using UnityEngine;

namespace Core.UI.Forms
{
    public interface IWorldSpaceForm<T> where T : MonoBehaviour
    {
        T AttachedTarget { get; }
        void AttachTo(T target);
    }
}