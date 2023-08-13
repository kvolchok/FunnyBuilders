using UnityEngine;

namespace DefaultNamespace
{
    [RequireComponent(typeof(Outline))]
    public class OutlineWorker : MonoBehaviour
    {
        [SerializeField] private int _highlightIntensity = 4;

        private Outline _outline;

        private void Awake()
        {
            _outline = GetComponent<Outline>();
        }

        private void OnMouseDown()
        {
            SetFocus();
        }

        private void OnMouseUp()
        {
            RemoveFocus();
        }

        private void SetFocus()
        {
            _outline.OutlineWidth = _highlightIntensity;
        }

        private void RemoveFocus()
        {
            _outline.OutlineWidth = 0;
        }
    }
}