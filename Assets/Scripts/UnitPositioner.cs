using DG.Tweening;
using UnityEngine;

public class UnitPositioner : MonoBehaviour
{
    [SerializeField]
    private Vector3 _offset;
    [SerializeField]
    private float _unitMovementDuration;

    public void PlaceUnitInHolder(Unit unit, UnitHolder unitHolder)
    {
        var targetPosition = unitHolder.transform.position + _offset;
        unit.transform.DOMove(targetPosition, _unitMovementDuration);
        unitHolder.SetUnit(unit);
    }
}