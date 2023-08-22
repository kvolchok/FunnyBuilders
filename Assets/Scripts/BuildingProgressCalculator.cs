using System;
using System.Collections;
using UnityEngine;

public class BuildingProgressCalculator : MonoBehaviour
{
    public event Action BuildingFinished;

    [SerializeField]
    private GameObject _building;
    
    private float _durationBuildingHeight;
    private float _endBuildingHeight;

    private Coroutine _buildCoroutine;
    
    public void Initialize(float durationBuildingHeight, float endBuildingHeight)
    {
        _durationBuildingHeight = durationBuildingHeight;
        _endBuildingHeight = endBuildingHeight;
    }
    
    public void Build(float height)
    {
        if (_buildCoroutine != null)
        {
            StopCoroutine(_buildCoroutine);
        }

        if (HasBuildingBuilt(_building.transform.localScale.y))
        {
            StopAllCoroutines();
            BuildingFinished?.Invoke();
            return;
        }

        var localScale = _building.transform.localScale;
        var finishScale = new Vector3(localScale.x, height, localScale.z);
        _buildCoroutine = StartCoroutine(Build(finishScale));
    }
    
    private bool HasBuildingBuilt(float height)
    {
        return height >= _endBuildingHeight;
    }

    private IEnumerator Build(Vector3 finishScale)
    {
        var currentTime = 0f;

        while (currentTime < _durationBuildingHeight)
        {
            var progress = currentTime / _durationBuildingHeight;

            var startScale = _building.transform.localScale;
            var currentScaleY = Vector3.Lerp(startScale, finishScale, progress);
            _building.transform.localScale = currentScaleY;
            currentTime += Time.deltaTime;
            yield return null;
        }

        _building.transform.localScale = finishScale;
    }
}