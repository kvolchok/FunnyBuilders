using System;
using UnityEngine;

public class MergeController : MonoBehaviour
{
    private int _maxUnitsLevel;

    public void Initialize(int maxUnitsLevel)
    {
        _maxUnitsLevel = maxUnitsLevel;
    }
    
    public void TryMergeUnits(UnitHolder currentUnitHolder, MergingPlace targetUnitHolder,
        Action<MergingPlace, Transform, int> onUnitsMerged, Action<Unit, UnitHolder> onUnitsNotMerged)
    {
        if (targetUnitHolder.Unit == null || currentUnitHolder == targetUnitHolder)
        {
            onUnitsNotMerged?.Invoke(currentUnitHolder.Unit, currentUnitHolder);
            return;
        }
        
        var targetUnitTransform = targetUnitHolder.Unit.transform;
        var targetUnitLevel = targetUnitHolder.Unit.Level;
        if (currentUnitHolder.Unit.Level == targetUnitLevel && currentUnitHolder.Unit.Level < _maxUnitsLevel)
        {
            currentUnitHolder.Unit.DestroyUnit();
            targetUnitHolder.Unit.DestroyUnit();
            currentUnitHolder.ClearFromUnit();
            
            onUnitsMerged?.Invoke(targetUnitHolder, targetUnitTransform, targetUnitLevel);
        }
        else
        {
            onUnitsNotMerged?.Invoke(currentUnitHolder.Unit, currentUnitHolder);
        }
    }
}