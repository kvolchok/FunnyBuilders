using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Building : MonoBehaviour, IUnitPositioner
{
    [field: SerializeField] public List<Tile> TilesList { get; private set; }

    [SerializeField] private int _buildingConstructionCost;
    [SerializeField] private int _amountAvailablePlaces;
    [SerializeField] private float _durationUnitReturn;
    
    private WalletManager _walletManager;
    private BuildingIncomeCalculator _buildingIncomeCalculator;
    private BuildingProgressCalculator _buildingProgressCalculator;

    private int _amountProfitAllUnits;

    public void Initialize(WalletManager walletManager, BuildingIncomeCalculator buildingIncomeCalculator,
        BuildingProgressCalculator buildingProgressCalculator)
    {
        _walletManager = walletManager;
        _buildingIncomeCalculator = buildingIncomeCalculator;
        _buildingProgressCalculator = buildingProgressCalculator;
        
        _buildingIncomeCalculator.MoneyEarned += OnMoneyEarned;
        _buildingProgressCalculator.BuildingFinished += OnBuildingFinished;
        
        ShowAvailablePlaces(_amountAvailablePlaces);
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

    public void BuyPlace()
    {
        foreach (var tile in TilesList)
        {
            if (tile.isActiveAndEnabled)
            {
                continue;
            }

            if (!tile.isActiveAndEnabled)
            {
                tile.gameObject.SetActive(true);
                return;
            }
        }
    }

    public void PlaceUnitOnTile(Unit unit, Tile targetTile)
    {
        Vector3 targetPosition = new Vector3(targetTile.transform.position.x, unit.transform.localScale.y,
            targetTile.transform.position.z);
        unit.transform.DOMove(targetPosition, _durationUnitReturn);
        targetTile.SetUnit(unit);
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

    private void ShowAvailablePlaces(int amountAvailablePlaces)
    {
        var count = 0;
        foreach (var tile in TilesList)
        {
            if (count >= amountAvailablePlaces)
            {
                tile.gameObject.SetActive(false);
            }

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