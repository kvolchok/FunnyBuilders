using DG.Tweening;
using UnityEngine;

public class UnitPositioner : MonoBehaviour
{
    [SerializeField]
    private float _movementDuration;

    public void PlaceUnitOnTile(Unit unit, Tile tile)
    {
        var targetPosition = new Vector3(tile.transform.position.x, unit.transform.localScale.y, 
            tile.transform.position.z);
        unit.transform.DOMove(targetPosition, _movementDuration);
        tile.SetUnit(unit);
    }
}