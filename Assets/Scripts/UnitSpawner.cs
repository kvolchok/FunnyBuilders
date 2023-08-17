using System.Linq;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    [SerializeField]
    private Unit _unitPrefab;

    private UnitSettings[] _unitSettings;

    public void Initialize(UnitSettings[] unitSettings)
    {
        _unitSettings = unitSettings;
    }
    
    public Unit SpawnUnit(Transform spawnPoint, int level = 1)
    {
        var unitSettings = _unitSettings.FirstOrDefault(unit => unit.Level == level);
        var unit = Instantiate(_unitPrefab);
        unit.Initialize(unitSettings);
        unit.transform.position = spawnPoint.position;
        return unit;
    }
}