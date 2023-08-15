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
    private UnitPositioner _unitPositioner;
    [SerializeField]
    private MergeController _mergeController;

    private void Awake()
    {
        _walletManager.Initialize(_gameSettings.StartMoney);
        _unitBuyer.Initialize(_walletManager, _gameSettings.UnitPrices);
        _spotBuyer.Initialize(_walletManager, _gameSettings.SpotPrices);
        _unitSpawner.Initialize(_gameSettings.UnitSettings);
        _mergeController.Initialize(_gameSettings.UnitSettings.Length);
        
        _unitsManager.Initialize(_unitSpawner, _unitPositioner, _mergeController);
    }
}