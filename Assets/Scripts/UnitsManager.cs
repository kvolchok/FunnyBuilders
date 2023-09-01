using System;
using JetBrains.Annotations;
using UnityEngine;

public class UnitsManager : MonoBehaviour
{
    public event Action<Unit, MergingPlace> UnitSpawned;
    public event Action<Unit> UnitsNotMerged;
    
    [SerializeField]
    private MergingPlacesController _mergingPlacesController;
    [SerializeField]
    private Transform _unitSpawnPoint;
    
    private UnitSpawner _unitSpawner;
    private MergeController _mergeController;

    public void Initialize(UnitSpawner unitSpawner, MergeController mergeController)
    {
        _unitSpawner = unitSpawner;
        _mergeController = mergeController;
    }
    
    [UsedImplicitly]
    public void AddNewUnit()
    {
        var unit = _unitSpawner.SpawnUnit(_unitSpawnPoint);
        var mergingPlace = _mergingPlacesController.GetFirstAvailable();
        UnitSpawned?.Invoke(unit, mergingPlace);
    }
    
    public void TryMergeUnits(Unit unit, MergingPlace targetMergingPlace)
    {
        _mergeController.TryMergeUnits(unit, targetMergingPlace, OnUnitsMerged, OnUnitsNotMerged);
    }
    
    private void OnUnitsMerged(MergingPlace targetMergingPlace, Transform targetUnitTransform, int targetUnitLevel)
    {
        var newUnit = _unitSpawner.SpawnUnit(targetUnitTransform, ++targetUnitLevel);
        targetMergingPlace.SetUnit(newUnit);
        targetMergingPlace.ShowMergeAnimation();
    }

    private void OnUnitsNotMerged(Unit unit)
    {
        UnitsNotMerged?.Invoke(unit);
    }
}