using UnityEngine;
using UnityEngine.Events;

namespace Wallet
{
    public class WalletManager : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent<int, int> _moneyValueChanged;
    
        private int _currentMoney;
    
        public void ChangeMoney(int deltaMoney)
        {
            var oldMoney = _currentMoney;
            _currentMoney += deltaMoney;
            _moneyValueChanged?.Invoke(oldMoney, _currentMoney);
        }

        public bool TryPurchase(int price)
        {
            if (!HasEnoughMoney(price))
            {
                return false;
            }

            ChangeMoney(-price);
            return true;
        }

        private bool HasEnoughMoney(int price)
        {
            return _currentMoney >= price;
        }
    }
}