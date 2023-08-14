using System;
using System.Collections;
using UnityEngine;

public class BuildingIncomeCalculator : MonoBehaviour
{
    [SerializeField] private int _paymentFirstLevelUnit;
    [SerializeField] private int _paymentSecondLevelUnit;
    private int _paymentInterval;
    private Action<int> _showMoneyOnDisplay;


    private void SetPaymentInterval(int workLevel)
    {
        CheckLevelWorker(workLevel);
        Debug.Log("StartMoneyCoroutine");
        StartCoroutine(GiveSalary());
    }

    private void CheckLevelWorker(int workLevel)
    {
        switch (workLevel)
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
        while (profit < 10000)
        {
            profit += _paymentInterval;
            _showMoneyOnDisplay.Invoke(_paymentInterval);
            yield return new WaitForSeconds(1.0f);
        }
        _showMoneyOnDisplay.Invoke(profit);
    }
    public void StopWorking()
    {
        StopAllCoroutines();
    }
    public void StartPay(int workLevel, Action<int> ShowMoneyOnDisplay)
    {
        Debug.Log("StartPay");
        _showMoneyOnDisplay = ShowMoneyOnDisplay;
        SetPaymentInterval(workLevel);
    }
}