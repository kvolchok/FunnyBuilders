using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private GameSettings _gameSettings;
    
    [SerializeField]
    private WalletManager _walletManager;
    [SerializeField]
    private EntityBuyer _unitBuyer;
    [SerializeField]
    private EntityBuyer _spotBuyer;
    
    [SerializeField]
    private UnitPositioner _unitPositioner;
    
    [SerializeField]
    private UnitsManager _unitsManager;
    [SerializeField]
    private UnitSpawner _unitSpawner;
    [SerializeField]
    private MergeController _mergeController;

    [SerializeField]
    private ConstructionManager _constructionManager;
    [SerializeField]
    private BuildingIncomeCalculator _buildingIncomeCalculator;
    [SerializeField]
    private BuildingConstruction _buildingConstruction;

    [SerializeField]
    private DragController _dragController;

    private void Awake()
    {
        _walletManager.ChangeMoney(_gameSettings.StartMoney);
        _unitBuyer.Initialize(_walletManager, _gameSettings.UnitPrices);
        _spotBuyer.Initialize(_walletManager, _gameSettings.SpotPrices);
        
        _unitSpawner.Initialize(_gameSettings.UnitSettings);
        _unitPositioner.Initialize(_gameSettings.UnitOffset, _gameSettings.UnitMovementDuration);
        _mergeController.Initialize(_gameSettings.UnitSettings.Length);
        _unitsManager.Initialize(_unitSpawner, _mergeController);
        
        _buildingIncomeCalculator.Initialize(_gameSettings.UnitPaymentInterval);
        _buildingConstruction.Initialize(_gameSettings.DurationBuildingHeight);
        _constructionManager.Initialize(_walletManager, _buildingIncomeCalculator,
            _buildingConstruction, _gameSettings.BuildingConstructionCost, _gameSettings.AmountAvailableSpots);

        _dragController.Initialize(_unitPositioner.UnitOffset);
        
        _dragController.CanTakeObject += OnCanTakeObject;
        _dragController.OnCantDropObject += OnCantDropObject;
        _unitsManager.UnitSpawned += OnUnitSpawned;
        _unitsManager.UnitsNotMerged += OnUnitsNotMerged;
        _constructionManager.UnitRecruited += OnUnitRecruited;
        _constructionManager.UnitDismissed += OnUnitDismissed;
    }

    private bool OnCanTakeObject(IDraggable draggedObject)
    {
        if (draggedObject is Unit unit)
        {
            return unit.State != UnitState.Work;
        }

        return false;
    }
    
    private void OnCantDropObject(Unit unit, DropPlace dropPlace)
    {
        _unitPositioner.PlaceUnitInHolder(unit, dropPlace);
    }
    
    private void OnUnitSpawned(Unit unit, MergingPlace targetPlace)
    {
        _unitPositioner.PlaceUnitInHolder(unit, targetPlace);
    }
    
    private void OnUnitsNotMerged(Unit unit)
    {
        var targetPlace = _dragController.InitialDropPlace;
        _unitPositioner.PlaceUnitInHolder(unit, targetPlace);
    }
    
    private void OnUnitRecruited(Unit unit, WorkPlace workPlace)
    {
        _unitPositioner.PlaceUnitInWorkPlace(unit, workPlace);
    }
    
    private void OnUnitDismissed(Unit unit)
    {
        var targetPlace = _dragController.InitialDropPlace;
        _unitPositioner.PlaceUnitInHolder(unit, targetPlace);
    }
    
    private void OnDestroy()
    {
        _dragController.CanTakeObject -= OnCanTakeObject;
        _dragController.OnCantDropObject -= OnCantDropObject;
        _unitsManager.UnitSpawned -= OnUnitSpawned;
        _unitsManager.UnitsNotMerged -= OnUnitsNotMerged;
        _constructionManager.UnitRecruited -= OnUnitRecruited;
        _constructionManager.UnitDismissed -= OnUnitDismissed;
    }
}