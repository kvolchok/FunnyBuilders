using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class BuildingProgressCalculator : MonoBehaviour
{
    public event Action BuildingFinished;
    
    [SerializeField]
    private UnityEvent<float> _onHeightChanged;
    
    private const float END_BUILDING_POSITION_Y = 0;

    [SerializeField]
    private GameObject _building;
    [SerializeField]
    private ParticleSystem _dustParticle;

    private float _durationBuildingHeight;
    private bool _isParticleSystemWorking;
    private float _depthExcavationForBuildingY;
    private Tweener _tweener;
    
    public void Initialize(float durationBuildingHeight)
    {
        _durationBuildingHeight = durationBuildingHeight;
        _depthExcavationForBuildingY = _building.transform.localPosition.y;
    }

    public void Build(float height)
    {
        if (_tweener != null)
        {
            DOTween.Kill(_tweener);
        }

        if (!_isParticleSystemWorking)
        {
            _isParticleSystemWorking = true;
            _dustParticle.Play();
        }

        var positionY = Mathf.Abs(_depthExcavationForBuildingY * height) + _depthExcavationForBuildingY;
        if (HasBuildingBuilt(positionY))
        {
            _dustParticle.Stop();
            BuildingFinished?.Invoke();
            return;
        }
        
        StartBuildProcess(positionY);
        _onHeightChanged?.Invoke(height);
    }

    private bool HasBuildingBuilt(float height)
    {
        return height >= END_BUILDING_POSITION_Y;
    }

    private void StartBuildProcess(float height)
    {
        var finishPosition = new Vector3(_building.transform.localPosition.x, height,
            _building.transform.localPosition.z);
        _tweener =_building.transform.DOLocalMove(finishPosition, _durationBuildingHeight);
    }
}