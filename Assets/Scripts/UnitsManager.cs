using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;

public class UnitsManager : MonoBehaviour
{
    [SerializeField]
    private GridController _gridController;
    [SerializeField]
    private UnitSpawner _unitSpawner;
    [SerializeField]
    private UnitPositioner _unitPositioner;
    [SerializeField]
    private MergeController _mergeController;
    [SerializeField]
    private Transform _spawnPoint;

    private void Awake()
    {
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
        _mergeController.TryMergeUnits(currentTile, targetTile, OnTriedMergeUnits);
    }
    
    private void OnTriedMergeUnits(Unit unit, Tile tile)
    {
        _unitPositioner.PlaceUnitOnTile(unit, tile);
    }
    
    private void OnDestroy()
    {
        _gridController.UnitDropped -= OnUnitDropped;
    }
}