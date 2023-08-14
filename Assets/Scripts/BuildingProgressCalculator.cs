using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class BuildingProgressCalculator : MonoBehaviour
{
    [SerializeField] private GameObject _buildingBuilt;
    [SerializeField] private float _durationBuilt;

    private const float _scaleY = 4.0f;
    private Action _stopWorking;
    private Coroutine _buildFloor;
    private Vector3 _transformLocalScale;

    private IEnumerator Build(float localScale)
    {
        var currentTime = 0f;
        while (currentTime < _durationBuilt)
        {
            var progress = currentTime / _durationBuilt;

            var scale = _buildingBuilt.transform.localScale;
            _transformLocalScale = scale;
            _transformLocalScale.y = Mathf.Lerp(scale.y, localScale, progress);
            _buildingBuilt.transform.localScale =
                new Vector3(_transformLocalScale.x, _transformLocalScale.y, _transformLocalScale.z);
            currentTime += Time.deltaTime;
            yield return null;
        }
    }

    private bool IsEnoughScale(float scale)
    {
        return scale >= _scaleY;
    }

    public void Initialize(Action StopWorking)
    {
        _stopWorking = StopWorking;
    }

    public void BuildFloor(float scale)
    {
        if (_buildFloor != null)
        {
            StopCoroutine(_buildFloor);
        }

        if (IsEnoughScale(_buildingBuilt.transform.localScale.y))
        {
            StopAllCoroutines();
            _stopWorking.Invoke();
            return;
        }

        _buildFloor = StartCoroutine(Build(scale));
    }
}