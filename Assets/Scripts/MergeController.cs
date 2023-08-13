using System;
using UnityEngine;

public class MergeController : MonoBehaviour
{
    public event Action<Unit, Unit, Unit> UnitsMerged;
    public event Action<Unit> UnitsNotMerged;
    
    [SerializeField]
    private UnitSpawner _unitSpawner;

    public void TryMergeUnits(Unit currentUnit, Unit targetUnit)
    {
        var targetUnitLevel = targetUnit.Level;
        if (currentUnit.Level == targetUnitLevel)
        {
            var newUnit = _unitSpawner.GetSpawnedUnit(targetUnit.transform, ++targetUnitLevel);
            UnitsMerged?.Invoke(currentUnit, targetUnit, newUnit);
        }
        else
        {
            UnitsNotMerged?.Invoke(currentUnit);
        }
    }
}