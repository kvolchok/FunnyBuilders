using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;

public class UnitsManager : MonoBehaviour, IUnitPositioner
{
    [SerializeField]
    private MergePlacesController _mergePlacesController;
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
        var tile = _mergePlacesController.GetFirstAvailable();
        PlaceUnitOnTile(unit, tile);
    }
    
    public void PlaceUnitOnTile(Unit unit, Tile tile)
    {
        var targetPosition = new Vector3(tile.transform.position.x, unit.transform.localScale.y, 
            tile.transform.position.z);
        unit.transform.DOMove(targetPosition, _unitMovementDuration);
        tile.SetUnit(unit);
    }
    
    public void TryMergeUnits(Tile currentTile, Tile targetTile)
    {
        _mergeController.TryMergeUnits(currentTile, targetTile, OnUnitsMerged, PlaceUnitOnTile);
    }
    
    private void OnUnitsMerged(Tile targetTile, Transform targetUnitTransform, int targetUnitLevel)
    {
        var newUnit = _unitSpawner.SpawnUnit(targetUnitTransform, ++targetUnitLevel);
        PlaceUnitOnTile(newUnit, targetTile);
    }
}