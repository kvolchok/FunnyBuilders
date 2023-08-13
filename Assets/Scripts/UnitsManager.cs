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
    
    private Vector3 _currentUnitStartPosition;

    private void Awake()
    {
        _gridController.UnitDropped += OnUnitDropped;
        _mergeController.UnitsMerged += OnUnitsMerged;
        _mergeController.UnitsNotMerged += OnUnitsNotMerged;
    }

    // Определить количество доступных к покупке юнитов
    public void BuyNewUnit()
    {
        var unit = _unitSpawner.SpawnUnit(_spawnPoint);
        var tile = _gridController.GetFirstAvailable();
        _unitPositioner.PlaceUnitOnTile(unit, tile);
    }

    private void OnUnitDropped(Tile currentTile, Tile targetTile)
    {
        _currentUnitStartPosition = currentTile.Unit.transform.position;
        
        currentTile.Unit.transform
            .DOMove(targetTile.Unit.transform.position, 1)
            .OnComplete(() => _mergeController.TryMergeUnits(currentTile, targetTile));
    }
    
    private void OnUnitsMerged(Unit newUnit, Tile targetTile)
    {
        _unitPositioner.PlaceUnitOnTile(newUnit, targetTile);
    }
    
    private void OnUnitsNotMerged(Unit unit)
    {
        unit.transform.DOMove(_currentUnitStartPosition, 1);
    }

    private void OnDestroy()
    {
        _gridController.UnitDropped -= OnUnitDropped;
        _mergeController.UnitsMerged -= OnUnitsMerged;
        _mergeController.UnitsNotMerged -= OnUnitsNotMerged;
    }
}