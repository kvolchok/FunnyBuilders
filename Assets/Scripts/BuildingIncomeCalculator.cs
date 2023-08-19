using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingIncomeCalculator : MonoBehaviour
{
    [SerializeField] private int _paymentFirstLevelUnit;
    [SerializeField] private int _paymentSecondLevelUnit;
    [SerializeField] private int _maxProfitUnit;
    [SerializeField] private float _waitingTimeBetweenUnitProfit;

    private int _paymentInterval;
    private Action<int> _showMoneyOnDisplay;
    private Dictionary<Transform, Coroutine> _dataBaseCoroutine = new Dictionary<Transform, Coroutine>();

    public void StopAllWork()
    {
        StopAllCoroutines();
    }

    public void StartPay(Unit currentUnit, Action<int> showMoneyOnDisplay)
    {
        _showMoneyOnDisplay = showMoneyOnDisplay;
        SetPaymentInterval(currentUnit);
    }

    public void StopPay(Unit dischargedUnit)
    {
        var coroutineDismissedUnit = _dataBaseCoroutine[dischargedUnit.transform];
        StopCoroutine(coroutineDismissedUnit);
    }
    private void SetPaymentInterval(Unit currentUnit)
    {
        CheckLevelWorker(currentUnit);
       var coroutine = StartCoroutine(GiveSalary());
       AddCoroutineToDataBase(currentUnit.transform, coroutine);
    }

    private void AddCoroutineToDataBase(Transform transform, Coroutine coroutine)
    {
        _dataBaseCoroutine.Add(transform, coroutine);
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