using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;

public class UnitsManager : MonoBehaviour, IUnitPositioner
{
    [SerializeField]
    private MergingPlacesController _mergingPlacesController;
    [SerializeField]
    private Transform _unitSpawnPoint;
    
    private UnitSpawner _unitSpawner;
    private MergeController _mergeController;
    private float _unitMovementDuration;

    public void Initialize(UnitSpawner unitSpawner, MergeController mergeController, float unitMovementDuration)
    {
        _unitSpawner = unitSpawner;
        _mergeController = mergeController;
        _unitMovementDuration = unitMovementDuration;
    }
    
    [UsedImplicitly]
    public void AddNewUnit()
    {
        var unit = _unitSpawner.SpawnUnit(_unitSpawnPoint);
        var mergingPlace = _mergingPlacesController.GetFirstAvailable();
        PlaceUnitInHolder(unit, mergingPlace);
    }
    
    public void PlaceUnitInHolder(Unit unit, UnitHolder unitHolder)
    {
        var targetPosition = new Vector3(unitHolder.transform.position.x, unit.transform.localScale.y, 
            unitHolder.transform.position.z);
        unit.transform.DOMove(targetPosition, _unitMovementDuration);
        unitHolder.SetUnit(unit);
    }
    
    public void TryMergeUnits(UnitHolder currentUnitHolder, MergingPlace targetUnitHolder)
    {
        _mergeController.TryMergeUnits(currentUnitHolder, targetUnitHolder, OnUnitsMerged, PlaceUnitInHolder);
    }
    
    private void OnUnitsMerged(MergingPlace targetUnitHolder, Transform targetUnitTransform, int targetUnitLevel)
    {
        var newUnit = _unitSpawner.SpawnUnit(targetUnitTransform, ++targetUnitLevel);
        PlaceUnitInHolder(newUnit, targetUnitHolder);
    }
}