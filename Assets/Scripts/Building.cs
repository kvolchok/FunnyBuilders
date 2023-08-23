using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Building : MonoBehaviour, IUnitPositioner
{
    [field: SerializeField]
    public List<Tile> WorkPlaces { get; private set; }
    
    private WalletManager _walletManager;
    private BuildingIncomeCalculator _buildingIncomeCalculator;
    private BuildingProgressCalculator _buildingProgressCalculator;
    
    private int _buildingConstructionCost;
    private int _amountAvailableWorkPlaces;
    private float _unitMovementDuration;

    private int _amountProfitAllUnits;

    public void Initialize(WalletManager walletManager, BuildingIncomeCalculator buildingIncomeCalculator,
        BuildingProgressCalculator buildingProgressCalculator, int buildingConstructionCost, int amountAvailableWorkPlaces,
        float unitMovementDuration)
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
    
    public void AddUnitToBuildingSite(Tile currentTile, Tile targetTile)
    {
        var draggableUnit = currentTile.Unit;
        var replacementUnit = targetTile.Unit;

        RecruitUnit(draggableUnit);
        PlaceUnitOnTile(draggableUnit, targetTile);

        if (replacementUnit != null)
        {
            DismissUnit(replacementUnit);
            PlaceUnitOnTile(replacementUnit, currentTile);
        }
        else
        {
            currentTile.ClearFromUnit();
        }
    }

    public void PlaceUnitOnTile(Unit unit, Tile tile)
    {
        var targetPosition = new Vector3(tile.transform.position.x, unit.transform.localScale.y,
            tile.transform.position.z);
        unit.transform.DOMove(targetPosition, _unitMovementDuration);
        tile.SetUnit(unit);
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
        _buildingProgressCalculator.Build(height, _buildingConstructionCost);
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