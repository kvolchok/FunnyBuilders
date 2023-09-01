using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Wallet;

namespace ConstructionSystem
{
    public class ConstructionManager : MonoBehaviour
    {
        public event Action<Unit, WorkPlace> UnitRecruited;
        public event Action<Unit> UnitDismissed;
    
        [field: SerializeField]
        public List<WorkPlace> WorkPlaces { get; private set; }

        [SerializeField]
        private Transform _foundation;

        private WalletManager _walletManager;
        private BuildingIncomeCalculator _buildingIncomeCalculator;
        private BuildingConstruction _buildingConstruction;

        private int _buildingConstructionCost;
        private int _amountAvailableWorkPlaces;
        private int _amountProfitAllUnits;

        public void Initialize(WalletManager walletManager, BuildingIncomeCalculator buildingIncomeCalculator,
            BuildingConstruction buildingConstruction, int buildingConstructionCost, int amountAvailableWorkPlaces)
        {
            _walletManager = walletManager;
            _buildingIncomeCalculator = buildingIncomeCalculator;
            _buildingConstruction = buildingConstruction;

            _buildingIncomeCalculator.MoneyEarned += OnMoneyEarned;
            _buildingConstruction.BuildingFinished += OnBuildingFinished;

            _buildingConstructionCost = buildingConstructionCost;
            _amountAvailableWorkPlaces = amountAvailableWorkPlaces;

            ShowAvailableWorkPlaces(_amountAvailableWorkPlaces);
        }

        [UsedImplicitly]
        public void BuyPlace()
        {
            foreach (var workPlace in WorkPlaces)
            {
                if (!workPlace.isActiveAndEnabled)
                {
                    workPlace.gameObject.SetActive(true);
                    return;
                }
            }
        }

        public void AddUnitToBuildingSite(Unit unit, WorkPlace targetWorkPlace)
        {
            var replacementUnit = targetWorkPlace.Unit;
            if (replacementUnit != null)
            {
                DismissUnit(replacementUnit);
                UnitDismissed?.Invoke(replacementUnit);
                targetWorkPlace.TurnOffSweatAnimation();
            }
        
            UnitRecruited?.Invoke(unit, targetWorkPlace);
            RecruitUnit(unit);
            targetWorkPlace.TurnOnSweatAnimation();
        }

        private void ShowAvailableWorkPlaces(int amountAvailablePlaces)
        {
            var count = 0;
            foreach (var workPlace in WorkPlaces)
            {
                workPlace.gameObject.SetActive(amountAvailablePlaces > count);
                count++;
            }
        }

        private void RecruitUnit(Unit recruitedUnit)
        {
            recruitedUnit.transform.LookAt(_foundation);
            recruitedUnit.ChangeState(UnitState.Work);
            _buildingIncomeCalculator.StartPay(recruitedUnit);
        }

        private void DismissUnit(Unit dismissedUnit)
        {
            dismissedUnit.ChangeState(UnitState.Idle);
            _buildingIncomeCalculator.StopPay(dismissedUnit);
        }

        private void OnMoneyEarned(int earnedMoney)
        {
            _amountProfitAllUnits += earnedMoney;
            _walletManager.ChangeMoney(earnedMoney);
            var progress = (float)_amountProfitAllUnits / _buildingConstructionCost;
            _buildingConstruction.Build(progress);
        }

        private void OnBuildingFinished()
        {
            _buildingIncomeCalculator.StopAllPayments();
            StopAllWorks();
        }

        private void StopAllWorks()
        {
            foreach (var workPlace in WorkPlaces)
            {
                if (workPlace.Unit != null)
                {
                    workPlace.TurnOffSweatAnimation();
                    workPlace.Unit.ChangeState(UnitState.Idle);
                }
            }
        }

        private void OnDestroy()
        {
            _buildingIncomeCalculator.MoneyEarned -= OnMoneyEarned;
            _buildingConstruction.BuildingFinished -= OnBuildingFinished;
        }
    }
}