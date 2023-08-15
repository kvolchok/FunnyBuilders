using System;
using UnityEngine;

public class MergeController : MonoBehaviour
{
    public void TryMergeUnits(Tile currentTile, Tile targetTile,
        Action<Tile, Transform, int> onUnitsMerged, Action<Unit, Tile> onUnitsNotMerged)
    {
        var targetUnitTransform = targetTile.Unit.transform;
        var targetUnitLevel = targetTile.Unit.Level;
        if (currentTile.Unit.Level == targetUnitLevel)
        {
            Destroy(currentTile.Unit.gameObject);
            Destroy(targetTile.Unit.gameObject);
            currentTile.ChangeState(true);
            
            onUnitsMerged?.Invoke(targetTile, targetUnitTransform, targetUnitLevel);
        }
        else
        {
            onUnitsNotMerged?.Invoke(currentTile.Unit, currentTile);
        }
    }
}