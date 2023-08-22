using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BuildingBar : MonoBehaviour
{
    [SerializeField] private Slider _buildingBar;
    [SerializeField] private float _durationFillBuildingBar;
    private float _maxValueBar;
    private Coroutine _fillBuildingBar;
    public void Initialize(float maxValueBar)
    {
        _maxValueBar = maxValueBar;
    }
    public void SetValueBar(float value)
    {
        if (_fillBuildingBar != null)
        {
            StopCoroutine(_fillBuildingBar);
        }
        var currentBuildingBarValue = value / _maxValueBar;
        _fillBuildingBar = StartCoroutine(SetBuildingBar(currentBuildingBarValue));

    }

    private IEnumerator SetBuildingBar (float currentBuildingBarValue)
    {
        var currentTime = 0f;

        while (currentTime < _durationFillBuildingBar)
        {
            var progress = currentTime / _durationFillBuildingBar;

            var startBuildingBarValue =  _buildingBar.value;
            var finishBuildingBarValue = currentBuildingBarValue;
            var currentValue = Mathf.Lerp(startBuildingBarValue, finishBuildingBarValue, progress);
            _buildingBar.value = currentValue;
            currentTime += Time.deltaTime;
            yield return null;
        }
        
    }
   
}
