using TMPro;
using UnityEngine;

public class WalletView : MonoBehaviour, INumberView
{
    [SerializeField]
    private TextMeshProUGUI _moneyLabel;

    public void UpdateView(int value)
    {
        _moneyLabel.text = value.ToString();
    }
}