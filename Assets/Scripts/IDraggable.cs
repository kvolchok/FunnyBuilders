using UnityEngine;

public interface IDraggable : IGetPosition
{
    void Drag(Vector3 targetPosition);
}