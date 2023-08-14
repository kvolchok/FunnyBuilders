using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;


public class Buildings : MonoBehaviour
{
    [SerializeField] private int _builderingPrice;
    [SerializeField] private List<Place> _placesList = new();
    [SerializeField] private BuildingIncomeCalculator _buildingIncomeCalculator;
    [SerializeField] private BuildingProgressCalculator _buildingProgressCalculator;
    [SerializeField] private WalletView _walletView;
    [SerializeField] private int _amountHiddenPlaces;
    private int _amountProfit;
    public List<Place> GetPlacesList => _placesList;

    private void Start()
    {
        HidePlaces();
        _buildingProgressCalculator.Initialize(_buildingIncomeCalculator.StopWorking);
    }

    private void HidePlaces()
    {
        var count = 0;
        foreach (var place in _placesList)
        {
            if (count >= _amountHiddenPlaces)
            {
                place.SetStatusPlace(false);
                place.gameObject.SetActive(false);
            }

            count++;
        }
    }
    private void ShowMoneyOnDisplay(int money)
    {
        _amountProfit += money;
        _walletView.UpdateMoneyView(_amountProfit);

        var scale = (float)_amountProfit / _builderingPrice;
        _buildingProgressCalculator.BuildFloor(scale);
    }

    public void AddBuilderToBuildingSite(Unit worker, Place place)
    {
        if (_placesList.Any(Place => Place.transform == place.transform))
        {
            place.SetStatusPlace(false);
            place.SetWorker(worker);
            _buildingIncomeCalculator.StartPay(worker.UnitLevel, ShowMoneyOnDisplay);
        }
    }
}