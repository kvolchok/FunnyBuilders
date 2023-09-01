using TMPro;
using UnityEngine;

namespace Ui.UnitLevelView
{
    public class UnitLevelView : MonoBehaviour, INumberView
    {
        [SerializeField]
        private TextMeshProUGUI _levelLabel;

        public void UpdateView(int value)
        {
            _levelLabel.text = value.ToString();
        }
    }
}