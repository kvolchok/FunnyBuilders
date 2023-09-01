using System.Collections.Generic;
using UnityEngine;

namespace Ui.UnitLevelView
{
    public class UnitLevelViewManager : MonoBehaviour
    {
        private readonly Dictionary<Unit, UnitLevelView> _unitLevelViewBindings = new();
    
        [SerializeField]
        private UiPositioner _uiPositioner;
        [SerializeField]
        private UnitLevelView _levelViewPrefab;

        public void CreateLevelView(Unit unit)
        {
            unit.UnitDestroyed += OnUnitDestroyed;
        
            var unitLevelView = Instantiate(_levelViewPrefab, transform);
            unitLevelView.UpdateView(unit.Level);
            _unitLevelViewBindings.Add(unit, unitLevelView);
        }

        private void Update()
        {
            foreach (var (unit, unitLevelView) in _unitLevelViewBindings)
            {
                _uiPositioner.UpdatePosition(unit.transform, unitLevelView.transform as RectTransform);
            }
        }

        private void OnUnitDestroyed(Unit unit)
        {
            unit.UnitDestroyed -= OnUnitDestroyed;
            _unitLevelViewBindings.Remove(unit, out var unitLevelView);
            Destroy(unitLevelView.gameObject);
        }
    }
}