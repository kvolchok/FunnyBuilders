using UnityEngine;
using UnityEngine.UI;

public class BuildingProgressBar : MonoBehaviour
{
    [SerializeField]
    private Slider _buildingProgressBar;
    [SerializeField]
    private Image _fill;
    [SerializeField]
    private Gradient _gradient;
    
    public void SetValueBar(float value)
    {
        _buildingProgressBar.value = value;
        var color = _gradient.Evaluate(_buildingProgressBar.value);
        _fill.color = color;
    }
}
