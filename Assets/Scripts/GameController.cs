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
        _unitsManager.Initialize(_unitSpawner, _unitPositioner, _mergeController);
        
        _buildingIncomeCalculator.Initialize(_gameSettings.UnitPaymentInterval);
        _buildingConstruction.Initialize(_gameSettings.DurationBuildingHeight);
        _constructionManager.Initialize(_walletManager, _unitPositioner, _buildingIncomeCalculator,
            _buildingConstruction, _gameSettings.BuildingConstructionCost, _gameSettings.AmountAvailableSpots);

        _dragController.Initialize(_unitPositioner);
    }
}