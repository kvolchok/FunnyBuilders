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
    private UnitsManager _unitsManager;
    [SerializeField]
    private UnitSpawner _unitSpawner;
    [SerializeField]
    private MergeController _mergeController;

    [SerializeField]
    private Building _building;
    [SerializeField]
    private BuildingIncomeCalculator _buildingIncomeCalculator;
    [SerializeField]
    private BuildingProgressCalculator _buildingProgressCalculator;

    [SerializeField]
    private DragController _dragController;

    private void Awake()
    {
        _walletManager.ChangeMoney(_gameSettings.StartMoney);
        _unitBuyer.Initialize(_walletManager, _gameSettings.UnitPrices);
        _spotBuyer.Initialize(_walletManager, _gameSettings.SpotPrices);
        
        _unitSpawner.Initialize(_gameSettings.UnitSettings);
        _mergeController.Initialize(_gameSettings.UnitSettings.Length);
        _unitsManager.Initialize(_unitSpawner, _mergeController, _gameSettings.UnitMovementDuration);
        
        _buildingIncomeCalculator.Initialize(_gameSettings.UnitPaymentInterval);
        _buildingProgressCalculator.Initialize(_gameSettings.DurationBuildingHeight,
            _gameSettings.EndBuildingHeight);
        _building.Initialize(_walletManager, _buildingIncomeCalculator, _buildingProgressCalculator,
            _gameSettings.BuildingConstructionCost, _gameSettings.AmountAvailableSpots,
            _gameSettings.UnitMovementDuration);

        _dragController.Initialize(_gameSettings.UnitMovementDuration);
    }
}