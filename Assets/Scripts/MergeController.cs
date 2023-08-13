using System;
using UnityEngine;

public class MergeController : MonoBehaviour
{
    public event Action<Unit, Tile> UnitsMerged;
    public event Action<Unit> UnitsNotMerged;
    
    [SerializeField]
    private UnitSpawner _unitSpawner;

    public void TryMergeUnits(Tile currentTile, Tile targetTile)
    {
        var targetUnitLevel = targetTile.Unit.Level;
        if (currentTile.Unit.Level == targetUnitLevel)
        {
            var newUnit = _unitSpawner.SpawnUnit(targetTile.Unit.transform, ++targetUnitLevel);
            Destroy(currentTile.Unit.gameObject);
            Destroy(targetTile.Unit.gameObject);
            currentTile.ChangeState(true);
            UnitsMerged?.Invoke(newUnit, targetTile);
        }
        else
        {
            UnitsNotMerged?.Invoke(currentTile.Unit);
        }
    }
}