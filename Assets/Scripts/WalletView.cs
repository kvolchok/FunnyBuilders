using System.Collections;
using TMPro;
using UnityEngine;

public class WalletView : MonoBehaviour, INumberView
{
    [SerializeField]
    private TextMeshProUGUI _moneyLabel;
    [SerializeField]
    private float _moneyCalculationTime;

    private Coroutine _currentCoroutine;

    public void UpdateView(int value)
    {
        _moneyLabel.text = value.ToString();
    }

    public void SetNewMoney(int oldMoney, int newMoney)
    {
        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
        }
        
        _currentCoroutine = StartCoroutine(ShowMoneyCalculationAnimation(oldMoney, newMoney));
    }
    
    private IEnumerator ShowMoneyCalculationAnimation(int oldMoney, int newMoney)
    {
        var currentTime = 0f;

        while (currentTime <= _moneyCalculationTime)
        {
            var progress = currentTime / _moneyCalculationTime;
            var currentMoney = (int)Mathf.Lerp(oldMoney, newMoney, progress);
            currentTime += Time.deltaTime;
            UpdateView(currentMoney);

            yield return null;
        }
        
        UpdateView(newMoney);
    }
}