using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;

public class UnitsManager : MonoBehaviour, IUnitPositioner
{
    [SerializeField]
    private GridController _gridController;
    [SerializeField]
    private Transform _spawnPoint;
    [SerializeField]
    private float _unitMovementDuration;
    
    private UnitSpawner _unitSpawner;
    private MergeController _mergeController;

    public void Initialize(UnitSpawner unitSpawner, MergeController mergeController)
    {
        _unitSpawner = unitSpawner;
        _mergeController = mergeController;
            
        _gridController.UnitDropped += OnUnitDropped;
    }
    
    [UsedImplicitly]
    public void AddNewUnit()
    {
        var unit = _unitSpawner.SpawnUnit(_spawnPoint);
        var tile = _gridController.GetFirstAvailable();
        PlaceUnitOnTile(unit, tile);
    }
    
    public void PlaceUnitOnTile(Unit unit, Tile tile)
    {
        var targetPosition = new Vector3(tile.transform.position.x, unit.transform.localScale.y, 
            tile.transform.position.z);
        unit.transform.DOMove(targetPosition, _unitMovementDuration);
        tile.SetUnit(unit);
    }

    private void OnUnitDropped(Tile currentTile, Tile targetTile)
    {
        currentTile.Unit.transform
            .DOMove(targetTile.Unit.transform.position, 1)
            .OnComplete(() => MergeUnits(currentTile, targetTile));
    }

    private void MergeUnits(Tile currentTile, Tile targetTile)
    {
        _mergeController.TryMergeUnits(currentTile, targetTile, OnUnitsMerged, PlaceUnitOnTile);
    }
    
    private void OnUnitsMerged(Tile targetTile, Transform targetUnitTransform, int targetUnitLevel)
    {
        var newUnit = _unitSpawner.SpawnUnit(targetUnitTransform, ++targetUnitLevel);
        PlaceUnitOnTile(newUnit, targetTile);
    }
    
    private void OnDestroy()
    {
        _gridController.UnitDropped -= OnUnitDropped;
    }
}