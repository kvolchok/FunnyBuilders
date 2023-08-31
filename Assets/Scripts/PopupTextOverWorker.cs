using UnityEngine;

namespace DefaultNamespace
{
    public class PopupTextOverWorker : MonoBehaviour
    {
        [field:SerializeField]
        public Vector3 Offset {get; private set;}
        
        [SerializeField]
        private TextMesh _floatingTextPrefab;
        
        private TextMesh currentFloatingText;
        
        public void ShowFloatingText(int salary)
        {
            currentFloatingText = Instantiate(_floatingTextPrefab, transform.position + Offset, Quaternion.identity);
            currentFloatingText.text = "+" + salary;
        }
        
        public void HideFloatingText()
        {
            if (currentFloatingText != null)
            {
                Destroy(currentFloatingText);
            }
        }
    }
}