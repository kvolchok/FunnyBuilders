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
    
    private IDraggable _draggedObject;
    private UnitHolder _initialUnitHolder;
    private UnitHolder _destinationUnitHolder;
    private bool _canDropObject;

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

        var draggedObject = hit.collider.GetComponent<IDraggable>();
        if (draggedObject == null)
        {
            return;
        }

        var initialUnitHolder = GetHolderUnderDraggedObject(draggedObject);
        if (initialUnitHolder == null)
        {
            return;
        }
        
        if (initialUnitHolder is MergingPlace)
        {
            _draggedObject = draggedObject;
            _initialUnitHolder = initialUnitHolder;
        }
    }

    private UnitHolder GetHolderUnderDraggedObject(IDraggable draggedObject)
    {
        var draggedObjectPosition = draggedObject.GetPosition();
        if (!Physics.Raycast(draggedObjectPosition, Vector3.down, out var hit,
                Mathf.Infinity))
        {
            return null;
        }

        var unitHolder = hit.collider.GetComponent<UnitHolder>();
        return unitHolder;
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
            _draggedObject.Drag(hit.point);
            var draggedObjectPosition = _draggedObject.GetPosition();
            _canDropObject = CanDropObject(draggedObjectPosition);
        }
    }

    private bool CanDropObject(Vector3 position)
    {
        var ray = new Ray(position, Vector3.down);
        if (!Physics.Raycast(ray, out var hitInfo))
        {
            return false;
        }

        var unitHolder = hitInfo.collider.GetComponent<UnitHolder>();
        if (unitHolder != null)
        {
            _canDropObject = true;
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

        if (_canDropObject)
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
            PlaceUnitInHolder(_draggedObject as Unit, _initialUnitHolder);
        }

        _draggedObject = null;
    }
}