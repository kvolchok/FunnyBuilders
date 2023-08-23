using System.Collections.Generic;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;

public class Building : MonoBehaviour, IUnitPositioner
{
    [field: SerializeField]
    public List<WorkPlace> WorkPlaces { get; private set; }
    
    private WalletManager _walletManager;
    private BuildingIncomeCalculator _buildingIncomeCalculator;
    private BuildingProgressCalculator _buildingProgressCalculator;
    
    private int _buildingConstructionCost;
    private int _amountAvailableWorkPlaces;
    private float _unitMovementDuration;

    private int _amountProfitAllUnits;

    public void Initialize(WalletManager walletManager, BuildingIncomeCalculator buildingIncomeCalculator,
        BuildingProgressCalculator buildingProgressCalculator, int buildingConstructionCost,
        int amountAvailableWorkPlaces, float unitMovementDuration)
    {
        _walletManager = walletManager;
        _buildingIncomeCalculator = buildingIncomeCalculator;
        _buildingProgressCalculator = buildingProgressCalculator;
        
        _buildingIncomeCalculator.MoneyEarned += OnMoneyEarned;
        _buildingProgressCalculator.BuildingFinished += OnBuildingFinished;

        _buildingConstructionCost = buildingConstructionCost;
        _amountAvailableWorkPlaces = amountAvailableWorkPlaces;
        _unitMovementDuration = unitMovementDuration;
        
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

        RecruitUnit(draggableUnit);
        PlaceUnitInHolder(draggableUnit, targetUnitHolder);

        if (replacementUnit != null)
        {
            DismissUnit(replacementUnit);
            PlaceUnitInHolder(replacementUnit, currentUnitHolder);
        }
        else
        {
            currentUnitHolder.ClearFromUnit();
        }
    }

    public void PlaceUnitInHolder(Unit unit, UnitHolder unitHolder)
    {
        var targetPosition = new Vector3(unitHolder.transform.position.x, unit.transform.localScale.y,
            unitHolder.transform.position.z);
        unit.transform.DOMove(targetPosition, _unitMovementDuration);
        unitHolder.SetUnit(unit);
    }

    private void DismissUnit(Unit dismissedUnit)
    {
        dismissedUnit.ChangeWorkingState(false);
        _buildingIncomeCalculator.StopPay(dismissedUnit);
    }

    private void RecruitUnit(Unit recruitedUnit)
    {
        recruitedUnit.ChangeWorkingState(true);
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