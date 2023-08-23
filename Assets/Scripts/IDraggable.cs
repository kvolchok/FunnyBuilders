using UnityEngine;

public interface IDraggable
{
    void Drag(Vector3 targetPosition);
    Vector3 GetPosition();
}