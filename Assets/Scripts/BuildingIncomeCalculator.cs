using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingIncomeCalculator : MonoBehaviour
{
    public event Action<int> MoneyEarned;

    private readonly Dictionary<Transform, Coroutine> _dataBaseCoroutine = new();

    private float _unitPaymentInterval;

    public void Initialize(float unitPaymentInterval)
    {
        _unitPaymentInterval = unitPaymentInterval;
    }
    
    public void StartPay(Unit currentUnit)
    {
        var coroutine = StartCoroutine(GiveSalary(currentUnit.Salary));
        AddCoroutineToDataBase(currentUnit.transform, coroutine);
    }

    public void StopPay(Unit dischargedUnit)
    {
        var coroutineDismissedUnit = _dataBaseCoroutine[dischargedUnit.transform];
        StopCoroutine(coroutineDismissedUnit);
        _dataBaseCoroutine.Remove(dischargedUnit.transform);
    }

    public void StopAllPayments()
    {
        StopAllCoroutines();
    }
    
    private IEnumerator GiveSalary(int unitSalary)
    {
        while (true)
        {
            MoneyEarned?.Invoke(unitSalary);
            yield return new WaitForSeconds(_unitPaymentInterval);
        }
    }

    /// <summary>
    /// this method have to check
    /// </summary>
    private void AddCoroutineToDataBase(Transform transform, Coroutine coroutine)
    {
        _dataBaseCoroutine.Add(transform, coroutine);
    }
}