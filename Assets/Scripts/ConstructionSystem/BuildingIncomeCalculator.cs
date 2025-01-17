using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ConstructionSystem
{
    public class BuildingIncomeCalculator : MonoBehaviour
    {
        public event Action<int> MoneyEarned;

        private readonly Dictionary<Unit, Coroutine> _dataBaseCoroutine = new();

        private float _unitPaymentInterval;

        public void Initialize(float unitPaymentInterval)
        {
            _unitPaymentInterval = unitPaymentInterval;
        }
    
        public void StartPay(Unit currentUnit)
        {
            var coroutine = StartCoroutine(GiveSalary(currentUnit.Salary));
            _dataBaseCoroutine.Add(currentUnit, coroutine);
        }

        public void StopPay(Unit dischargedUnit)
        {
            var coroutineDismissedUnit = _dataBaseCoroutine[dischargedUnit];
            StopCoroutine(coroutineDismissedUnit);
            _dataBaseCoroutine.Remove(dischargedUnit);
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
    }
}