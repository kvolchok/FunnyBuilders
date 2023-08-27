using UnityEngine;

public class UiPositioner : MonoBehaviour
{
    [SerializeField]
    private Transform _buildingPosition;
    [SerializeField]
    private RectTransform _buildingBar;
    [SerializeField]
    private Vector3 _offset;

    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
        var pointScreenSpace = RectTransformUtility.WorldToScreenPoint(_camera, _buildingPosition.position + _offset);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent as RectTransform, pointScreenSpace,
            null, out var localPoint);
        _buildingBar.anchoredPosition = localPoint;
    }
}
