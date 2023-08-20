using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Building : MonoBehaviour, IUnitPositioner
{
    [field: SerializeField] public List<Tile> TilesList { get; private set; }

    [SerializeField] private int _buildingConstructionCost;

    [SerializeField] private BuildingIncomeCalculator _buildingIncomeCalculator;
    [SerializeField] private BuildingProgressCalculator _buildingProgressCalculator;
    [SerializeField] private WalletManager _walletManager;
    [SerializeField] private int _amountAvailablePlaces;
    [SerializeField] private float _durationUnitReturn;

    private int _amountProfitAllUnits;

    private void Start()
    {
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

    private void OnMoneyEarned(int money)
    {
        _amountProfitAllUnits += money;
        _walletManager.AddMoney(money);
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