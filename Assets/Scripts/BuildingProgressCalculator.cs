using System;
using DG.Tweening;
using UnityEngine;

public class BuildingProgressCalculator : MonoBehaviour
{
    [SerializeField] private GameObject _buildingBuilt;
    [SerializeField] private float _durationBuilt;

    private const float _scaleY = 4.0f;
    private Action _stopWorking;

    public void Initialize(Action StopWorking)
    {
        _stopWorking = StopWorking;
    }

    public void BuildFloor(float scale)
    {
        var localScale = _buildingBuilt.transform.localScale;
        var scaleY = localScale.y + scale; 
       _buildingBuilt.transform.DOScale(new Vector3(localScale.x, scaleY,localScale.z), _durationBuilt); 
       if ( IsEnoughtScale(scaleY))
        {
            _stopWorking.Invoke();
        }

       
    }

    private bool IsEnoughtScale(float scale)
    {
        if (scale >= _scaleY)
        {
            return true;
        }

        return false;
    }
}