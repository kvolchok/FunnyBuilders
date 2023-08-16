using TMPro;
using UnityEngine;

public class WalletView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _moneyLabel;

    public void UpdateMoneyView(int value)
    {
        _moneyLabel.text = value.ToString();
    }
}