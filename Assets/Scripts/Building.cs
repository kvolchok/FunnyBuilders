using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Building : MonoBehaviour, IUnitPositioner
{
    [field: SerializeField] public List<Tile> TilesList { get; private set; }

    [SerializeField] private int _builderingPrice;

    [SerializeField] private BuildingIncomeCalculator _buildingIncomeCalculator;
    [SerializeField] private BuildingProgressCalculator _buildingProgressCalculator;
    [SerializeField] private WalletManager _walletManager;
    [SerializeField] private int _amountAvalliablePlaces;
    [SerializeField] private float _durationUnitReturn;

    private int _amountProfit;

    public void AddUnitToBuildingSite(Tile currentTile, Tile targetTile)
    {
        var draggableUnit = currentTile.Unit;
        var replacementUnit = targetTile.Unit;
        RecruitUnit(draggableUnit, targetTile);
        if (replacementUnit != null)
        {
            FiredUnit(replacementUnit);

            PlaceUnitOnTile(replacementUnit, currentTile);
        }
        
        currentTile.ClearFromUnit();
    }

    private void FiredUnit(Unit dischargedUnit)
    {
        dischargedUnit.ChangeWorkingState(false);
        _buildingIncomeCalculator.StopPay(dischargedUnit);
    }

    private void RecruitUnit(Unit recruitUnit, Tile targetTile)
    {
        targetTile.SetUnit(recruitUnit);
        recruitUnit.ChangeWorkingState(true);
        _buildingIncomeCalculator.StartPay(recruitUnit, ShowMoneyOnDisplay);
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


    private void Start()
    {
        HidePlaces();
        _buildingProgressCalculator.Initialize(_buildingIncomeCalculator.StopAllWork);
    }

    private void HidePlaces()
    {
        var count = 0;
        foreach (var tile in TilesList)
        {
            if (count >= _amountAvalliablePlaces)
            {
                tile.gameObject.SetActive(false);
            }

            count++;
        }
    }

    private void ShowMoneyOnDisplay(int money)
    {
        _amountProfit += money;
        _walletManager.AddMoney(money);
        var scaleY = (float)_amountProfit / _builderingPrice;
        _buildingProgressCalculator.BuildFloor(scaleY);
    }

    public void PlaceUnitOnTile(Unit unit, Tile targetTile)
    {
        Vector3 targetPosition = new Vector3(targetTile.transform.position.x, unit.transform.localScale.y,
            targetTile.transform.position.z);
        unit.transform.DOMove(targetPosition, _durationUnitReturn);
        targetTile.SetUnit(unit);
    }
}