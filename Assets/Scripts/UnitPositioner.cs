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
    
    public void PlaceUnitInHolder(Unit unit, DropPlace dropPlace)
    {
        unit.ChangeState(UnitState.Run);
        var targetPosition = dropPlace.transform.position + UnitOffset;
        unit.transform.LookAt(targetPosition);
        unit.transform
            .DOMove(targetPosition, _unitMovementDuration)
            .OnComplete(() => unit.ChangeState(UnitState.Idle));
        dropPlace.SetUnit(unit);
    }
    
    public void PlaceUnitInWorkPlace(Unit unit, WorkPlace workPlace)
    {
        var targetPosition = workPlace.transform.position + UnitOffset;
        unit.transform.LookAt(targetPosition);
        unit.transform.position = targetPosition;
        workPlace.SetUnit(unit);
    }
}