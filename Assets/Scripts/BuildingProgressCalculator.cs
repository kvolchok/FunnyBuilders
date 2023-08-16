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

    public void Initialize(Action stopWorking)
    {
        _stopWorking = stopWorking;
    }

    public void BuildFloor(float scaleY)
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

        var localScale = _buildingBuilt.transform.localScale;
        var finishScale = new Vector3(localScale.x, scaleY, localScale.z);
        _buildFloor = StartCoroutine(Build(finishScale));
    }

    private IEnumerator Build(Vector3 endScale)
    {
        var currentTime = 0f;

        while (currentTime < _durationBuilt)
        {
            var progress = currentTime / _durationBuilt;

            var startScale = _buildingBuilt.transform.localScale;
            var currentScaleY = Vector3.Lerp(startScale, endScale, progress);
            _buildingBuilt.transform.localScale = currentScaleY;
            currentTime += Time.deltaTime;
            yield return null;
        }

        _buildingBuilt.transform.localScale = endScale;
    }

    private bool IsEnoughScale(float scale)
    {
        return scale >= _scaleY;
    }
}