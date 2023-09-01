using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Wallet;

namespace BuyButton
{
    public class EntityBuyer : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent<int> _entityPriceChanged;
        [SerializeField]
        private UnityEvent _entityBought;
    
        private int _currentPrice => _entityPrices[_currentPriceIndex];
    
        [SerializeField]
        private Button _buyButton;
    
        private WalletManager _walletManager;
        private int[] _entityPrices;
        private int _currentPriceIndex;

        public void Initialize(WalletManager walletManager, int[] entityPrices)
        {
            _walletManager = walletManager;
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
}