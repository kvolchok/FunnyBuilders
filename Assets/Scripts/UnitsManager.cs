using DG.Tweening;
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

    private Unit[,] _units;
    private Vector3Int _targetTileIndex;
    private Vector3 _currentUnitStartPosition;

    private void Awake()
    {
        _units = new Unit[_gridController.MapSize.x, _gridController.MapSize.y];
        
        _gridController.UnitDropped += UnitDropped;
        _mergeController.UnitsMerged += OnUnitsMerged;
        _mergeController.UnitsNotMerged += OnUnitsNotMerged;
    }

    // Определить количество доступных к покупке юнитов
    public void BuyNewUnit()
    {
        var unit = _unitSpawner.GetSpawnedUnit(_spawnPoint);
        var tile = _gridController.GetFirstAvailable();
        _unitPositioner.PlaceUnitOnTile(unit, tile);
        var index = _gridController.GetIndex(tile);
        _units[index.x, index.z] = unit;
    }

    private void UnitDropped(Vector3Int currentTileIndex, Vector3Int targetTileIndex)
    {
        _targetTileIndex = targetTileIndex;
        var currentUnit = _units[currentTileIndex.x, currentTileIndex.z];
        _currentUnitStartPosition = currentUnit.transform.position;
        var targetUnit = _units[targetTileIndex.x, targetTileIndex.z];
        
        currentUnit.transform
            .DOMove(targetUnit.transform.position, 1)
            .OnComplete(() => _mergeController.TryMergeUnits(currentUnit, targetUnit));
    }
    
    private void OnUnitsMerged(Unit currentUnit, Unit targetUnit, Unit newUnit)
    {
        Destroy(currentUnit.gameObject);
        Destroy(targetUnit.gameObject);
        _units[_targetTileIndex.x, _targetTileIndex.z] = newUnit;
        _gridController.ChangeCurrentTileState(true);
    }
    
    private void OnUnitsNotMerged(Unit unit)
    {
        unit.transform.DOMove(_currentUnitStartPosition, 1);
    }

    private void OnDestroy()
    {
        _gridController.UnitDropped -= UnitDropped;
        _mergeController.UnitsMerged -= OnUnitsMerged;
        _mergeController.UnitsNotMerged -= OnUnitsNotMerged;
    }
}