using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class BuildingProgressCalculator : MonoBehaviour
{
    public event Action BuildingFinished;

    [SerializeField] private GameObject _building;
    [SerializeField] private UnityEvent<float, float> _setValueOnBuildingBar;

    private float _durationBuildingHeight;
    private const float _positionBuildingY = 0;
    private float _depthExcavationForBuildingY; 
    private Tweener _tweener;
   

    public void Initialize(float durationBuildingHeight, float endBuildingHeight)
    {
        _durationBuildingHeight = durationBuildingHeight;
        _depthExcavationForBuildingY = _building.transform.localPosition.y;
    }

    public void Build(float height, float buildingConstructionCost)
    {
      
        if (_tweener != null)
        {
            DOTween.Kill(_tweener);
        }
        var localPosition = _building.transform.localPosition;
        var positionY = Mathf.Abs(_depthExcavationForBuildingY*height)+_depthExcavationForBuildingY;
        if (positionY > 0)
        {
            positionY = 0;
            if (HasBuildingBuilt(positionY))
            {
                BuildingFinished?.Invoke();
                return;
            }
        }

        var finishPosition = new Vector3(localPosition.x, positionY, localPosition.z);
         StartBuildProcess(finishPosition);
        _setValueOnBuildingBar?.Invoke(height, buildingConstructionCost);
    }

    private bool HasBuildingBuilt(float height)
    {
        return height >= _positionBuildingY;
    }

    private void StartBuildProcess(Vector3 finishPosition)
    {
        _building.transform.DOLocalMove(finishPosition, _durationBuildingHeight);
    }
   
}