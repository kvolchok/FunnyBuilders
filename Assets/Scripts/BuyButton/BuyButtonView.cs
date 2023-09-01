using TMPro;
using Ui;
using UnityEngine;

namespace BuyButton
{
    public class BuyButtonView : MonoBehaviour, INumberView
    {
        [SerializeField]
        private TextMeshProUGUI _priceLabel;

        public void UpdateView(int value)
        {
            _priceLabel.text = value.ToString();
        }
    }
}