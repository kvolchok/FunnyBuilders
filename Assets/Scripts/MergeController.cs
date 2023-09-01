using System;
using UnityEngine;

public class MergeController : MonoBehaviour
{
    private int _maxUnitsLevel;

    public void Initialize(int maxUnitsLevel)
    {
        _maxUnitsLevel = maxUnitsLevel;
    }
    
    public void TryMergeUnits(DropPlace currentDropPlace, MergingPlace targetMergingPlace,
        Action<MergingPlace, Transform, int> onUnitsMerged, Action<Unit, DropPlace> onUnitsNotMerged)
    {
        if (targetMergingPlace.Unit == null || currentDropPlace == targetMergingPlace)
        {
            onUnitsNotMerged?.Invoke(currentDropPlace.Unit, currentDropPlace);
            return;
        }
        
        var targetUnitTransform = targetMergingPlace.Unit.transform;
        var targetUnitLevel = targetMergingPlace.Unit.Level;
        if (currentDropPlace.Unit.Level == targetUnitLevel && currentDropPlace.Unit.Level < _maxUnitsLevel)
        {
            currentDropPlace.Unit.DestroyUnit();
            targetMergingPlace.Unit.DestroyUnit();
            currentDropPlace.ClearFromUnit();
            
            onUnitsMerged?.Invoke(targetMergingPlace, targetUnitTransform, targetUnitLevel);
        }
        else
        {
            onUnitsNotMerged?.Invoke(currentDropPlace.Unit, currentDropPlace);
        }
    }
}