using System;
using UnityEngine;

public class MergeController : MonoBehaviour
{
    [SerializeField]
    private UnitSpawner _unitSpawner;

    public void TryMergeUnits(Tile currentTile, Tile targetTile, Action<Unit, Tile> onTriedMergeUnits)
    {
        var targetUnitLevel = targetTile.Unit.Level;
        if (currentTile.Unit.Level == targetUnitLevel)
        {
            var newUnit = _unitSpawner.SpawnUnit(targetTile.Unit.transform, ++targetUnitLevel);
            Destroy(currentTile.Unit.gameObject);
            Destroy(targetTile.Unit.gameObject);
            currentTile.ChangeState(true);
            onTriedMergeUnits?.Invoke(newUnit, targetTile);
        }
        else
        {
            onTriedMergeUnits?.Invoke(currentTile.Unit, currentTile);
        }
    }
}