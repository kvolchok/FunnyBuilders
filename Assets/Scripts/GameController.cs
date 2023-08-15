using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private GameSettings _gameSettings;
    [SerializeField]
    private UnitSpawner _unitSpawner;
    [SerializeField]
    private UnitBuyer _unitBuyer;
    [SerializeField]
    private WalletManager _walletManager;

    private void Awake()
    {
        _unitSpawner.Initialize(_gameSettings.UnitSettings);
        _unitBuyer.Initialize(_gameSettings.UnitPrices);
        _walletManager.Initialize(_gameSettings.StartMoney);
    }
}