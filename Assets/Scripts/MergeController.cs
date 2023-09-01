using System;
using UnityEngine;

public class MergeController : MonoBehaviour
{
    private int _maxUnitsLevel;

    public void Initialize(int maxUnitsLevel)
    {
        _maxUnitsLevel = maxUnitsLevel;
    }
    
    public void TryMergeUnits(Unit unit, MergingPlace targetMergingPlace,
        Action<MergingPlace, Transform, int> onUnitsMerged, Action<Unit> onUnitsNotMerged)
    {
        if (targetMergingPlace.Unit == null || unit == targetMergingPlace.Unit)
        {
            onUnitsNotMerged?.Invoke(unit);
            return;
        }
        
        var targetUnitTransform = targetMergingPlace.Unit.transform;
        var targetUnitLevel = targetMergingPlace.Unit.Level;
        if (unit.Level == targetUnitLevel && unit.Level < _maxUnitsLevel)
        {
            unit.DestroyUnit();
            targetMergingPlace.Unit.DestroyUnit();
            
            onUnitsMerged?.Invoke(targetMergingPlace, targetUnitTransform, targetUnitLevel);
        }
        else
        {
            onUnitsNotMerged?.Invoke(unit);
        }
    }
}