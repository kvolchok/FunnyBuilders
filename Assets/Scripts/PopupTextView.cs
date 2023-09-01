using UnityEngine;

public class PopupTextView : MonoBehaviour
{
    [field: SerializeField]
    public Vector3 Offset { get; private set; }
    [SerializeField]
    private TextMesh _popupTextPrefab;

    private TextMesh _popupText;

    public void ShowPopupText(int salary)
    {
        _popupText = Instantiate(_popupTextPrefab, transform.position + Offset, Quaternion.identity);
        _popupText.text = "+" + salary;
    }

    public void HidePopupText()
    {
        if (_popupText != null)
        {
            Destroy(_popupText);
        }
    }
}