using System;
using System.Collections;
using UnityEngine;

public class BuildingIncomeCalculator : MonoBehaviour
{
    private int _paymentInterval;
    private Action<int> _showMoneyOnDisplay;
    
    public void StartPay(int workLevel, Action<int> ShowMoneyOnDisplay)
    {
        Debug.Log("StartPay");
        _showMoneyOnDisplay = ShowMoneyOnDisplay;
        SetPaymentInterval(workLevel);
    }

    private void SetPaymentInterval(int workLevel)
    {
        CheckLevelWorker(workLevel);
        
         StartCoroutine(GiveSalary());
    }

    private void CheckLevelWorker(int workLevel)
    {
        switch (workLevel)
        {
            case 1:
                _paymentInterval = 10;
                break;
            case 2:
                _paymentInterval = 20;
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
            yield return new WaitForSecondsRealtime(0.5f);
        }

        _showMoneyOnDisplay.Invoke(_paymentInterval);
    }

    public void StopWorking()
    {
        StopAllCoroutines();
    }
}