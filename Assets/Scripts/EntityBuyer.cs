using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EntityBuyer : MonoBehaviour
{
    [SerializeField]
    private UnityEvent<int> _entityPriceChanged;
    [SerializeField]
    private UnityEvent _entityBought;
    
    [SerializeField]
    private WalletManager _walletManager;
    [SerializeField]
    private Button _buyButton;

    private int _currentPrice => _entityPrices[_currentPriceIndex];
    private int[] _entityPrices;
    private int _currentPriceIndex;

    public void Initialize(int[] entityPrices)
    {
        _entityPrices = entityPrices;
        SetEntityPrice(_currentPrice);
    }

    [UsedImplicitly]
    public void BuyNewEntity()
    {
        if (!_walletManager.TryPurchase(_currentPrice))
        {
            return;
        }
        
        _entityBought?.Invoke();
        
        if (_currentPriceIndex >= _entityPrices.Length - 1)
        {
            _buyButton.interactable = false;
            return;
        }
        
        _currentPriceIndex++;
        SetEntityPrice(_currentPrice);
    }
    
    private void SetEntityPrice(int currentPrice)
    {
        _entityPriceChanged?.Invoke(currentPrice);
    }
}