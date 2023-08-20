using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingIncomeCalculator : MonoBehaviour
{
    public event Action<int> MoneyEarned;

    [SerializeField] private int _paymentFirstLevelUnit;
    [SerializeField] private int _paymentSecondLevelUnit;
    [SerializeField] private float _waitingTimeBetweenUnitProfit;

    private int _paymentInterval;
    private readonly Dictionary<Transform, Coroutine> _dataBaseCoroutine = new ();

    public void StopAllPayments()
    {
        StopAllCoroutines();
    }

    public void StartPay(Unit currentUnit)
    {
        SetPaymentInterval(currentUnit);
    }

    public void StopPay(Unit dischargedUnit)
    {
        var coroutineDismissedUnit = _dataBaseCoroutine[dischargedUnit.transform];
        StopCoroutine(coroutineDismissedUnit);
        _dataBaseCoroutine.Remove(dischargedUnit.transform);
    }

    private void SetPaymentInterval(Unit currentUnit)
    {
        CheckLevelWorker(currentUnit);
        var coroutine = StartCoroutine(GiveSalary());
        AddCoroutineToDataBase(currentUnit.transform, coroutine);
    }

    /// <summary>
    /// this method have to check
    /// </summary>
    private void AddCoroutineToDataBase(Transform transform, Coroutine coroutine)
    {
        _dataBaseCoroutine.Add(transform, coroutine);
    }

    /// <summary>
    /// this method will changing
    /// </summary>
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
        while (true)
        {
            MoneyEarned?.Invoke(_paymentInterval);
            yield return new WaitForSeconds(_waitingTimeBetweenUnitProfit);
        }
    }
}