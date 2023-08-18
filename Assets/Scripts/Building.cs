using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Building : MonoBehaviour
{
    [field: SerializeField]
    public List<Tile> TilesList { get; private set; }
    
    [SerializeField] private int _builderingPrice;

    [SerializeField] private BuildingIncomeCalculator _buildingIncomeCalculator;
    [SerializeField] private BuildingProgressCalculator _buildingProgressCalculator;
    [SerializeField] private WalletManager _walletManager;
    [SerializeField] private int _amountHiddenPlaces;

    private int _amountProfit;

    public void AddUnitToBuildingSite(Unit unit, Tile tile)
    {
        if (TilesList.Any(place => place.transform == tile.transform))
        {
            tile.ChangeState(false);
            tile.SetUnit(unit);
            _buildingIncomeCalculator.StartPay(unit.Level, ShowMoneyOnDisplay);
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
                break;
            }
        }
    }

    private void Start()
    {
        HidePlaces();
        _buildingProgressCalculator.Initialize(_buildingIncomeCalculator.StopWorking);
    }

    private void HidePlaces()
    {
        var count = 0;
        foreach (var tile in TilesList)
        {
            if (count >= _amountHiddenPlaces)
            {
                tile.ChangeState(false);
                tile.gameObject.SetActive(false);
            }

            count++;
        }
    }

    private void ShowMoneyOnDisplay(int money)
    {
        _amountProfit += money;
        //_walletManager.UpdateMoneyView(_amountProfit); 
        var scaleY = (float)_amountProfit / _builderingPrice;
        _buildingProgressCalculator.BuildFloor(scaleY);
    }
}