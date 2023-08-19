using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class DragController : MonoBehaviour, IUnitPositioner
{
    [SerializeField] private UnityEvent<Tile, Tile> _dropOnWorkPlace;
    [SerializeField] private UnityEvent<Tile, Tile> _dropOnMergePlace;
    [SerializeField] private LayerMask _raycastLayerMask;
    [SerializeField] private float _movementDuration = 1f;
    [SerializeField] private Vector3 _offset;
    private Unit _draggedObject;
    private Tile _dropSpot;
    private Camera _camera;
    private bool _canDrop;
    private Tile _startTile;

    private void Awake()
    {
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
        if (Physics.Raycast(ray, out var hit))
        {
            var unit = hit.collider.GetComponent<Unit>();
            if (unit != null)
            {
                _draggedObject = unit;
                FindTileUnderDraggedObject();
            }
        }
    }

    private void FindTileUnderDraggedObject()
    {
        RaycastHit tileHit;
        if (Physics.Raycast(_draggedObject.transform.position, Vector3.down, out tileHit, Mathf.Infinity))
        {
            var tileComponent = tileHit.collider.GetComponent<Tile>();
            if (tileComponent != null)
            {
                _startTile = tileComponent;
            }
        }
    }

    private void DragObject()
    {
        var ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hit, Mathf.Infinity, _raycastLayerMask))
        {
            if (_draggedObject != null)
            {
                Vector3 newPosition = new Vector3(hit.point.x, hit.point.y, hit.point.z) + _offset;
                _draggedObject.transform.position = newPosition;
                _canDrop = CanDrop(newPosition);
            }
        }
    }

    private bool CanDrop(Vector3 position)
    {
        var ray = new Ray(position, Vector3.down);
        if (Physics.Raycast(ray, out var hitInfo))
        {
            var tile = hitInfo.collider.GetComponent<Tile>();
            if (tile != null)
            {
                _canDrop = true;
                _dropSpot = tile;
                return true;
            }
        }

        return false;
    }

    private void DropObject()
    {
        if (_draggedObject == null)
        {
            return;
        }

        if (_canDrop)
        {
            if (_dropSpot.IsMergePlace)
            {
                _dropOnMergePlace?.Invoke(_startTile, _dropSpot);
            }
            else if (_dropSpot.IsWorkPlace)
            {
                _dropOnWorkPlace?.Invoke(_startTile, _dropSpot);
            }
        }
        else
        {
            PlaceUnitOnTile(_draggedObject, _startTile);
        }


        _draggedObject = null;
    }

    public void PlaceUnitOnTile(Unit unit, Tile tile)
    {
        var targetPosition = tile.transform.position + _offset;
        unit.transform.DOMove(targetPosition, _movementDuration);
    }
}