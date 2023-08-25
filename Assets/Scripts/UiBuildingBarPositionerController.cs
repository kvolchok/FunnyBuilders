using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class UiBuildingBarPositionerController : MonoBehaviour
{
    [SerializeField] private Transform _buildingPosition;

    [SerializeField] private RectTransform _buildingBar;

    [SerializeField] private Vector3 _offset;
    // Update is called once per frame
    void Update()
    {
        var pointScreenSpace = RectTransformUtility.WorldToScreenPoint(Camera.main, _buildingPosition.position + _offset);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent as RectTransform, pointScreenSpace,
            null, out var localPoint);
        _buildingBar.anchoredPosition = localPoint;
    }
}
