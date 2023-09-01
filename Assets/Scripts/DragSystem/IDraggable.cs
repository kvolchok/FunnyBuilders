using UnityEngine;

namespace DragSystem
{
    public interface IDraggable
    {
        Transform Transform { get; }
    
        void Drag(Vector3 targetPosition);
    }
}