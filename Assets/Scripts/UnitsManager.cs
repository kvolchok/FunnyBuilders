using JetBrains.Annotations;
using UnityEngine;

public class UnitsManager : MonoBehaviour
{
    [SerializeField]
    private MergingPlacesController _mergingPlacesController;
    [SerializeField]
    private Transform _unitSpawnPoint;
    
    private UnitSpawner _unitSpawner;
    private UnitPositioner _unitPositioner;
    private MergeController _mergeController;

    public void Initialize(UnitSpawner unitSpawner, UnitPositioner unitPositioner,
        MergeController mergeController)
    {
        _unitSpawner = unitSpawner;
        _unitPositioner = unitPositioner;
        _mergeController = mergeController;
    }
    
    [UsedImplicitly]
    public void AddNewUnit()
    {
        var unit = _unitSpawner.SpawnUnit(_unitSpawnPoint);
        var mergingPlace = _mergingPlacesController.GetFirstAvailable();
        _unitPositioner.PlaceUnitInHolder(unit, mergingPlace);
    }
    
    public void TryMergeUnits(DropPlace currentDropPlace, MergingPlace targetMergingPlace)
    {
        _mergeController.TryMergeUnits(currentDropPlace, targetMergingPlace, OnUnitsMerged, 
            (unit, mergingPlace) => _unitPositioner.PlaceUnitInHolder(unit, mergingPlace));
    }
    
    private void OnUnitsMerged(MergingPlace targetMergingPlace, Transform targetUnitTransform, int targetUnitLevel)
    {
        var newUnit = _unitSpawner.SpawnUnit(targetUnitTransform, ++targetUnitLevel);
        targetMergingPlace.SetUnit(newUnit);
        targetMergingPlace.ShowMergeAnimation();
    }
}