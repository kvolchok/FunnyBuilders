using UnityEngine;

public interface IDraggable
{
    Transform Transform { get; }
    
    void Drag(Vector3 targetPosition);
}