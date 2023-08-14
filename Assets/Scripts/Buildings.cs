using DG.Tweening;
using UnityEngine;


public class Buildings : MonoBehaviour
{
    [SerializeField] private Grid _grid;
    [SerializeField] private int _builderingPrice;
    [SerializeField] private Place _placePrefab;
    [SerializeField] private BuildingIncomeCalculator _buildingIncomeCalculator;
    [SerializeField] private BuildingProgressCalculator _buildingProgressCalculator;
    [SerializeField] private WalletView _walletView;
    private Place[] _arrayPlaces;
    private int _amount;


    // Start is called before the first frame update
    private void Start()
    {
        _arrayPlaces = new Place[4];
        InstancePlaces();
        _buildingProgressCalculator.Initialize(_buildingIncomeCalculator.StopWorking);
    }

    private void InstancePlaces()
    {
        for (var i = 0; i < _arrayPlaces.Length; i++)
        {
            var place = Instantiate(_placePrefab);
            place.SetStatusPlace(true);

            var position = _grid.CellToWorld(new Vector3Int(i, 0, -i));

            if (i > 1)
            {
                place.gameObject.SetActive(false);
            }

            place.transform.position = position;

            _arrayPlaces[i] = place;
        }
    }

    public void AddBuilderToBuildingSite(UnityEngine.Vector3 position, Unit worker)
    {
        Vector3Int index = _grid.WorldToCell(position);
        var x = index.x;
        if (x is >= 0 and < 3)
        {
            if (_arrayPlaces[x]._isAvaliablePlace)
            {
                Debug.Log("StartWorking");
                _arrayPlaces[x].SetStatusPlace(false);
                _arrayPlaces[x].SetWorker(worker);
                _buildingIncomeCalculator.StartPay(worker.UnitLevel, ShowMoneyOnDisplay);
            }
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void ShowMoneyOnDisplay(int money)
    {
        _amount += money;
        _walletView.UpdateMoneyView(_amount);
       // if (_amount >= _builderingPrice)
        //{
            var scale = (float) _amount/_builderingPrice;
            _buildingProgressCalculator.BuildFloor(scale);
        //}
    }

    private bool IsProfitEnought(int profit)
    {
        if (profit % 200 == 0)
        {
            return true;
        }

        return false;
    }
}