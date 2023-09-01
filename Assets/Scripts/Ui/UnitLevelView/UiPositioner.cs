using UnityEngine;

namespace Ui.UnitLevelView
{
    public class UiPositioner : MonoBehaviour
    {
        [SerializeField]
        private Vector3 _offset;
    
        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
        }

        public void UpdatePosition(Transform gameEntity, RectTransform uiElement)
        {
            var screenPoint = RectTransformUtility.WorldToScreenPoint(_camera,
                gameEntity.position + _offset);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform, screenPoint,
                null, out var localPoint);
            uiElement.anchoredPosition = localPoint;
        }
    }
}