using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Building : MonoBehaviour
{
    [field: SerializeField]
    public List<WorkPlace> WorkPlaces { get; private set; }

    [SerializeField]
    private Transform _foundation;
    
    private WalletManager _walletManager;
    private UnitPositioner _unitPositioner;
    private BuildingIncomeCalculator _buildingIncomeCalculator;
    private BuildingProgressCalculator _buildingProgressCalculator;
    
    private int _buildingConstructionCost;
    private int _amountAvailableWorkPlaces;
    private int _amountProfitAllUnits;

    public void Initialize(WalletManager walletManager, UnitPositioner unitPositioner,
        BuildingIncomeCalculator buildingIncomeCalculator, BuildingProgressCalculator buildingProgressCalculator,
        int buildingConstructionCost, int amountAvailableWorkPlaces)
    {
        _walletManager = walletManager;
        _unitPositioner = unitPositioner;
        _buildingIncomeCalculator = buildingIncomeCalculator;
        _buildingProgressCalculator = buildingProgressCalculator;
        
        _buildingIncomeCalculator.MoneyEarned += OnMoneyEarned;
        _buildingProgressCalculator.BuildingFinished += OnBuildingFinished;

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
    
    public void AddUnitToBuildingSite(UnitHolder currentUnitHolder, WorkPlace targetUnitHolder)
    {
        var draggableUnit = currentUnitHolder.Unit;
        var replacementUnit = targetUnitHolder.Unit;
        
        _unitPositioner.PlaceUnitInWorkPlace(draggableUnit, targetUnitHolder);
        RecruitUnit(draggableUnit);

        if (replacementUnit != null)
        {
            DismissUnit(replacementUnit);
            _unitPositioner.PlaceUnitInHolder(replacementUnit, currentUnitHolder);
        }
        else
        {
            currentUnitHolder.ClearFromUnit();
        }
    }

    private void DismissUnit(Unit dismissedUnit)
    {
        dismissedUnit.ChangeState(UnitState.Idle);
        _buildingIncomeCalculator.StopPay(dismissedUnit);
    }

    private void RecruitUnit(Unit recruitedUnit)
    {
        recruitedUnit.transform.LookAt(_foundation);
        recruitedUnit.ChangeState(UnitState.Work);
        _buildingIncomeCalculator.StartPay(recruitedUnit);
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

    private void OnMoneyEarned(int earnedMoney)
    {
        _amountProfitAllUnits += earnedMoney;
        _walletManager.ChangeMoney(earnedMoney);
        var height = (float)_amountProfitAllUnits / _buildingConstructionCost;
        _buildingProgressCalculator.Build(height);
    }

    private void OnBuildingFinished()
    {
        _buildingIncomeCalculator.StopAllPayments();
    }

    private void OnDestroy()
    {
        _buildingIncomeCalculator.MoneyEarned -= OnMoneyEarned;
        _buildingProgressCalculator.BuildingFinished -= OnBuildingFinished;
    }
}