using UnityEngine;

namespace Animations
{
    public class PopupTextAnimation : MonoBehaviour
    {
        [field: SerializeField]
        public Vector3 Offset { get; private set; }
        [SerializeField]
        private TextMesh _popupTextPrefab;

        private TextMesh _popupText;

        public void TurnOnAnimation(int salary)
        {
            _popupText = Instantiate(_popupTextPrefab, transform.position + Offset, Quaternion.identity);
            _popupText.text = "+" + salary;
        }

        public void TurnOffAnimation()
        {
            if (_popupText != null)
            {
                Destroy(_popupText);
            }
        }
    }
}