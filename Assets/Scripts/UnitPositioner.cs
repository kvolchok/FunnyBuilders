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
        unit.ChangeState(UnitState.Run);
        var targetPosition = unitHolder.transform.position + UnitOffset;
        unit.transform.LookAt(targetPosition);
        unit.transform
            .DOMove(targetPosition, _unitMovementDuration)
            .OnComplete(() => unit.ChangeState(UnitState.Idle));
        unitHolder.SetUnit(unit);
    }
    
    public void PlaceUnitInWorkPlace(Unit unit, WorkPlace unitHolder)
    {
        var targetPosition = unitHolder.transform.position + UnitOffset;
        unit.transform.LookAt(targetPosition);
        unit.transform.position = targetPosition;
        unitHolder.SetUnit(unit);
    }
}