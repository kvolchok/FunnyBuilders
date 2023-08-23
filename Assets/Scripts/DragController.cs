using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class DragController : MonoBehaviour, IUnitPositioner
{
    [SerializeField]
    private UnityEvent<UnitHolder, MergingPlace> _dropOnMergingPlace;
    [SerializeField]
    private UnityEvent<UnitHolder, WorkPlace> _dropOnWorkPlace;
    
    [SerializeField]
    private LayerMask _planeLayerMask;

    private float _unitMovementDuration;
    private Camera _camera;
    
    private Unit _draggedObject;
    private UnitHolder _initialUnitHolder;
    private UnitHolder _destinationUnitHolder;
    private bool _canDropUnit;

    public void Initialize(float unitMovementDuration)
    {
        _unitMovementDuration = unitMovementDuration;
        _camera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TakeObject();
        }

        if (Input.GetMouseButton(0))
        {
            DragObject();
        }

        if (Input.GetMouseButtonUp(0))
        {
            DropObject();
        }
    }

    public void PlaceUnitInHolder(Unit unit, UnitHolder unitHolder)
    {
        var targetPosition = new Vector3(unitHolder.transform.position.x, unit.transform.localScale.y,
            unitHolder.transform.position.z);
        unit.transform.DOMove(targetPosition, _unitMovementDuration);
    }

    private void TakeObject()
    {
        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out var hit))
        {
            return;
        }

        var unit = hit.collider.GetComponent<Unit>();

        if (unit != null && !unit.IsWorking)
        {
            _draggedObject = unit;
            FindHolderUnderDraggedObject();
        }
    }

    private void FindHolderUnderDraggedObject()
    {
        if (!Physics.Raycast(_draggedObject.transform.position, Vector3.down, out var hit,
                Mathf.Infinity))
        {
            return;
        }

        var unitHolder = hit.collider.GetComponent<UnitHolder>();
        if (unitHolder != null)
        {
            _initialUnitHolder = unitHolder;
        }
    }

    private void DragObject()
    {
        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out var hit, Mathf.Infinity, _planeLayerMask))
        {
            return;
        }

        if (_draggedObject != null)
        {
            var currentPosition = new Vector3(hit.point.x, _draggedObject.transform.localScale.y, hit.point.z);
            _draggedObject.transform.position = currentPosition;
            _canDropUnit = CanDropUnit(currentPosition);
        }
    }

    private bool CanDropUnit(Vector3 position)
    {
        var ray = new Ray(position, Vector3.down);
        if (!Physics.Raycast(ray, out var hitInfo))
        {
            return false;
        }

        var unitHolder = hitInfo.collider.GetComponent<UnitHolder>();
        if (unitHolder != null)
        {
            _canDropUnit = true;
            _destinationUnitHolder = unitHolder;
            return true;
        }

        return false;
    }

    private void DropObject()
    {
        if (_draggedObject == null)
        {
            return;
        }

        if (_canDropUnit)
        {
            if (_destinationUnitHolder is MergingPlace mergingPlace)
            {
                _dropOnMergingPlace?.Invoke(_initialUnitHolder, mergingPlace);
            }
            else if (_destinationUnitHolder is WorkPlace workPlace)
            {
                _dropOnWorkPlace?.Invoke(_initialUnitHolder, workPlace);
            }
        }
        else
        {
            PlaceUnitInHolder(_draggedObject, _initialUnitHolder);
        }

        _draggedObject = null;
    }
}