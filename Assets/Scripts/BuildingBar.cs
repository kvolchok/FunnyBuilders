using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BuildingBar : MonoBehaviour
{
    [SerializeField] private Slider _buildingBar;
    private float _maxValueBar;
    private Coroutine _fillBuildingBar;
    
    public void SetValueBar(float value, float maxValueBar)
    {
    
     var settingValue = value ;
     _buildingBar.value = settingValue ;

    }

   
   
}
