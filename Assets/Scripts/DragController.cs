using UnityEngine;
using UnityEngine.Events;

public class DragController : MonoBehaviour
{
    [SerializeField]
    private UnityEvent<DropPlace, MergingPlace> _dropOnMergingPlace;
    [SerializeField]
    private UnityEvent<DropPlace, WorkPlace> _dropOnWorkPlace;
    
    [SerializeField]
    private LayerMask _planeLayerMask;

    private UnitPositioner _unitPositioner;
    private Camera _camera;
    
    private IDraggable _draggedObject;
    private DropPlace _initialDropPlace;
    private DropPlace _destinationDropPlace;
    private bool _canDropObject;

    public void Initialize(UnitPositioner unitPositioner)
    {
        _unitPositioner = unitPositioner;
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
            _initialDropPlace = initialUnitHolder;
        }
    }

    private DropPlace GetHolderUnderDraggedObject(IDraggable draggedObject)
    {
        var draggedObjectPosition = draggedObject.GetPosition();
        if (!Physics.Raycast(draggedObjectPosition, Vector3.down, out var hit,
                Mathf.Infinity))
        {
            return null;
        }

        var unitHolder = hit.collider.GetComponent<DropPlace>();
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
            var dragPoint = hit.point + _unitPositioner.UnitOffset;
            _draggedObject.Drag(dragPoint);
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

        var unitHolder = hitInfo.collider.GetComponent<DropPlace>();
        if (unitHolder != null)
        {
            _canDropObject = true;
            _destinationDropPlace = unitHolder;
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
            if (_destinationDropPlace is MergingPlace mergingPlace)
            {
                _dropOnMergingPlace?.Invoke(_initialDropPlace, mergingPlace);
            }
            else if (_destinationDropPlace is WorkPlace workPlace)
            {
                _dropOnWorkPlace?.Invoke(_initialDropPlace, workPlace);
            }
        }
        else
        {
            var currentUnit = _draggedObject as Unit;
            _unitPositioner.PlaceUnitInHolder(currentUnit, _initialDropPlace);
        }

        _draggedObject = null;
    }
}