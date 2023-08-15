using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UnitBuyer : MonoBehaviour
{
    [SerializeField]
    private UnityEvent<int> _unitPriceChanged;
    [SerializeField]
    private UnityEvent _unitBought;
    
    [SerializeField]
    private WalletManager _walletManager;
    [SerializeField]
    private Button _buyButton;

    private int _currentPrice => _unitPrices[_currentPriceIndex];
    private int[] _unitPrices;
    private int _currentPriceIndex;

    public void Initialize(int[] unitPrices)
    {
        _unitPrices = unitPrices;
        _unitPriceChanged?.Invoke(_currentPrice);
    }
    
    [UsedImplicitly]
    public void BuyNewUnit()
    {
        if (!_walletManager.TryPurchase(_currentPrice))
        {
            return;
        }
        
        _unitBought?.Invoke();
        
        if (_currentPriceIndex >= _unitPrices.Length - 1)
        {
            _buyButton.interactable = false;
            return;
        }
        
        _currentPriceIndex++;
        _unitPriceChanged?.Invoke(_currentPrice);
    }
}