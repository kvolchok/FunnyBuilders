using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class Building : MonoBehaviour
{
    [field: SerializeField]
    public List<Tile> TilesList { get; private set; }
    
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
        if (replacementUnit == null)
        {
              targetTile.SetUnit(draggableUnit);
            _buildingIncomeCalculator.StartPay(draggableUnit.Level, ShowMoneyOnDisplay);
        }

        if (replacementUnit != null)
        {
        }
    }

    private void ReturnUnit(Tile targetTile, Unit returnableUnit)
    {
        returnableUnit.gameObject.transform.DOMove(targetTile.transform.position, _durationUnitReturn);
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
        //_walletManager.AddMoney(_amountProfit); 
        var scaleY = (float)_amountProfit / _builderingPrice;
        _buildingProgressCalculator.BuildFloor(scaleY);
    }
}