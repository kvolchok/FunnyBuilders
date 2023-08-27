using UnityEngine;
using UnityEngine.UI;

public class BuildingBar : MonoBehaviour
{
    [SerializeField]
    private Slider _buildingBar;
    [SerializeField]
    private Image _fill;
    [SerializeField]
    private Gradient _gradient;
    
    public void SetValueBar(float value)
    {
        _buildingBar.value = value;
    }

    private void Update()
    {
        var color = _gradient.Evaluate(_buildingBar.value);
        _fill.color = color;
    }
}
