using TMPro;
using UnityEngine;

public class UnitLevelView : MonoBehaviour, INumberView
{
    [SerializeField]
    private TextMeshProUGUI _levelLabel;

    public void UpdateView(int value)
    {
        _levelLabel.text = value.ToString();
    }
}