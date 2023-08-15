using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;

public class UnitsManager : MonoBehaviour
{
    [SerializeField]
    private GridController _gridController;
    [SerializeField]
    private Transform _spawnPoint;
    
    private UnitSpawner _unitSpawner;
    private UnitPositioner _unitPositioner;
    private MergeController _mergeController;

    public void Initialize(UnitSpawner unitSpawner, UnitPositioner unitPositioner, MergeController mergeController)
    {
        _unitSpawner = unitSpawner;
        _unitPositioner = unitPositioner;
        _mergeController = mergeController;
            
        _gridController.UnitDropped += OnUnitDropped;
    }
    
    [UsedImplicitly]
    public void AddNewUnit()
    {
        var unit = _unitSpawner.SpawnUnit(_spawnPoint);
        var tile = _gridController.GetFirstAvailable();
        _unitPositioner.PlaceUnitOnTile(unit, tile);
    }

    private void OnUnitDropped(Tile currentTile, Tile targetTile)
    {
        currentTile.Unit.transform
            .DOMove(targetTile.Unit.transform.position, 1)
            .OnComplete(() => MergeUnits(currentTile, targetTile));
    }

    private void MergeUnits(Tile currentTile, Tile targetTile)
    {
        _mergeController.TryMergeUnits(currentTile, targetTile, OnUnitsMerged, OnUnitsNotMerged);
    }
    
    private void OnUnitsMerged(Tile targetTile, Transform targetUnitTransform, int targetUnitLevel)
    {
        var newUnit = _unitSpawner.SpawnUnit(targetUnitTransform, ++targetUnitLevel);
        _unitPositioner.PlaceUnitOnTile(newUnit, targetTile);
    }
    
    private void OnUnitsNotMerged(Unit currentUnit, Tile currentTile)
    {
        _unitPositioner.PlaceUnitOnTile(currentUnit, currentTile);
    }
    
    private void OnDestroy()
    {
        _gridController.UnitDropped -= OnUnitDropped;
    }
}