using DG.Tweening;
using UnityEngine;

public class UnitPositioner : MonoBehaviour
{
    private Vector3 _unitOffset;
    private float _unitSpeed;
    
    public void Initialize(Vector3 unitOffset, float unitSpeed)
    {
        _unitOffset = unitOffset;
        _unitSpeed = unitSpeed;
    }
    
    public void PlaceUnit(Unit unit, DropPlace dropPlace)
    {
        unit.ChangeState(UnitState.Run);
        var targetPosition = dropPlace.transform.position + _unitOffset;
        var travelDistance = Vector3.Distance(unit.transform.position, targetPosition);
        var travelTime = travelDistance / _unitSpeed;
        unit.transform.LookAt(targetPosition);
        unit.transform
            .DOMove(targetPosition, travelTime)
            .OnComplete(() => unit.ChangeState(UnitState.Idle));
        dropPlace.SetUnit(unit);
    }
    
    public void PlaceUnitInWorkPlace(Unit unit, WorkPlace workPlace)
    {
        var targetPosition = workPlace.transform.position + _unitOffset;
        unit.transform.LookAt(targetPosition);
        unit.transform.position = targetPosition;
        workPlace.SetUnit(unit);
    }
}