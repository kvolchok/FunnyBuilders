using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BuildingBar : MonoBehaviour
{
    [SerializeField] private Slider _buildingBar;
   
    
    public void SetValueBar(float value)
    {
    
    
     _buildingBar.value = value ;

    }

   
   
}
