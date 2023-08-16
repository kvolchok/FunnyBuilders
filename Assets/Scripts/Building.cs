using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Building : MonoBehaviour
{
    [SerializeField] private int _builderingPrice;

    [SerializeField] private BuildingIncomeCalculator _buildingIncomeCalculator;
    [SerializeField] private BuildingProgressCalculator _buildingProgressCalculator;
    [SerializeField] private WalletManager _walletManager;
    [SerializeField] private int _amountHiddenPlaces;

    private int _amountProfit;

    [field: SerializeField] public List<Tile> TilesList { get; private set; }

    public void AddUnitToBuildingSite(Unit worker, Tile tile)
    {
        if (TilesList.Any(Place => Place.transform == tile.transform))
        {
            tile.SetStatusPlace(false);
            tile.SetWorker(worker);
            _buildingIncomeCalculator.StartPay(worker.UnitLevel, ShowMoneyOnDisplay);
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
                tile.SetStatusPlace(false);
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