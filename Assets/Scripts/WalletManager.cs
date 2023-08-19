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

    public bool TryPurchase(int price)
    {
        if (!HasEnoughMoney(price))
        {
            return false;
        }
        
        var newMoney = _money - price;
        SetMoney(newMoney);
        return true;
    }

    public void AddMoney(int earnedMoney)
    {
        var newMoney = _money + earnedMoney;
        SetMoney(newMoney);
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