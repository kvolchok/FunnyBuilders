using DG.Tweening;
using UnityEngine;

public class UnitPositioner : MonoBehaviour
{
    public Vector3 UnitOffset { get; private set; }
    
    private float _unitMovementDuration;
    
    public void Initialize(Vector3 unitOffset, float unitMovementDuration)
    {
        UnitOffset = unitOffset;
        _unitMovementDuration = unitMovementDuration;
    }
    
    public void PlaceUnitInHolder(Unit unit, UnitHolder unitHolder)
    {
        var targetPosition = unitHolder.transform.position + UnitOffset;
        unit.transform.DOMove(targetPosition, _unitMovementDuration);
        unitHolder.SetUnit(unit);
    }
}