using System.Linq;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    [SerializeField]
    private GameSettings _gameSettings;
    [SerializeField]
    private Unit _unitPrefab;

    public Unit GetSpawnedUnit(Transform spawnPoint, int level = 1)
    {
        var unitSettings = _gameSettings.UnitSettings.FirstOrDefault(unit => unit.Level == level);
        var unit = Instantiate(_unitPrefab);
        unit.Initialize(unitSettings);
        unit.transform.position = spawnPoint.position;
        return unit;
    }
}