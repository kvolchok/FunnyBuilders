using UnityEngine;
using UnityEngine.Events;

public class WalletManager : MonoBehaviour
{
    [SerializeField]
    private UnityEvent<int> _moneyValueChanged;
    
    private int _money;

    public void Initialize(int money)
    {
        SetMoney(money);
    }

    public void TryBuyItem(int price)
    {
        if (HasEnoughMoney(price))
        {
            var newMoney = _money - price;
            SetMoney(newMoney);
        }
    }
    
    private void SetMoney(int money)
    {
        _money = money;
        _moneyValueChanged?.Invoke(_money);
    }

    private bool HasEnoughMoney(int price)
    {
        return _money >= price;
    }
}