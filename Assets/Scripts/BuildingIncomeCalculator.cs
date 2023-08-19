using System;
using System.Collections;
using UnityEngine;

public class BuildingIncomeCalculator : MonoBehaviour
{
    [SerializeField] private int _paymentFirstLevelUnit;
    [SerializeField] private int _paymentSecondLevelUnit;
    [SerializeField] private int _maxProfitUnit;
    [SerializeField] private float _waitingTimeBetweenUnitProfit;

    private int _paymentInterval;
    private Action<int> _showMoneyOnDisplay;

    public void StopAllWork()
    {
        StopAllCoroutines();
    }

    public void StartPay(Unit currentUnit, Action<int> showMoneyOnDisplay)
    {
        _showMoneyOnDisplay = showMoneyOnDisplay;
        SetPaymentInterval(currentUnit);
    }

    public void StopPay()
    {
        
    }
    private void SetPaymentInterval(Unit currentUnit)
    {
        CheckLevelWorker(currentUnit);
        StartCoroutine(GiveSalary());
    }

    private void CheckLevelWorker(Unit currentUnit)
    {
        var unitLevel = currentUnit.Level;
        switch (unitLevel)
        {
            case 1:
                _paymentInterval = _paymentFirstLevelUnit;
                break;
            case 2:
                _paymentInterval = _paymentSecondLevelUnit;
                break;
        }
    }

    private IEnumerator GiveSalary()
    {
        var profit = 0;
        while (profit < _maxProfitUnit)
        {
            profit += _paymentInterval;
            _showMoneyOnDisplay.Invoke(_paymentInterval);
            yield return new WaitForSeconds(_waitingTimeBetweenUnitProfit);
        }

        _showMoneyOnDisplay.Invoke(profit);
    }
}