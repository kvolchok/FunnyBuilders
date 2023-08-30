using UnityEngine;

public class UiPositioner : MonoBehaviour
{
    [SerializeField]
    private Transform _anchore;
    [SerializeField]
    private Vector3 _offset;
    [SerializeField]
    private RectTransform _uiElement;

    private void Start()
    {
        var camera = Camera.main;
        var screenPoint = RectTransformUtility.WorldToScreenPoint(camera, _anchore.position + _offset);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent as RectTransform, screenPoint,
            null, out var localPoint);
        _uiElement.anchoredPosition = localPoint;
    }
}
