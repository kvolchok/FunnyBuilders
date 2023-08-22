using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class DragController : MonoBehaviour, IUnitPositioner
{
    [SerializeField]
    private UnityEvent<Tile, Tile> _dropOnWorkPlace;
    [SerializeField]
    private UnityEvent<Tile, Tile> _dropOnMergePlace;
    
    [SerializeField]
    private LayerMask _planeLayerMask;

    private float _unitMovementDuration;
    private Camera _camera;
    
    private Unit _draggedObject;
    private Tile _destinationTile;
    private Tile _initialTile;
    private bool _canDropUnitOnTile;

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

    public void PlaceUnitOnTile(Unit unit, Tile tile)
    {
        var targetPosition = new Vector3(tile.transform.position.x, unit.transform.localScale.y,
            tile.transform.position.z);
        // var targetPosition = tile.transform.position + _offset;
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
            FindTileUnderDraggedObject();
        }
    }

    private void FindTileUnderDraggedObject()
    {
        if (!Physics.Raycast(_draggedObject.transform.position, Vector3.down, out var hit,
                Mathf.Infinity))
        {
            return;
        }

        var tile = hit.collider.GetComponent<Tile>();
        if (tile != null)
        {
            _initialTile = tile;
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
            //hit.point + _offset;
            // new Vector3(hit.point.x, _draggedObject.transform.localScale.y, hit.point.z);
            _draggedObject.transform.position = currentPosition;
            _canDropUnitOnTile = CanDropUnitOnTile(currentPosition);
        }
    }

    private bool CanDropUnitOnTile(Vector3 position)
    {
        var ray = new Ray(position, Vector3.down);
        if (!Physics.Raycast(ray, out var hitInfo))
        {
            return false;
        }

        var tile = hitInfo.collider.GetComponent<Tile>();
        if (tile != null)
        {
            _canDropUnitOnTile = true;
            _destinationTile = tile;
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

        if (_canDropUnitOnTile)
        {
            if (_destinationTile.IsMergePlace)
            {
                _dropOnMergePlace?.Invoke(_initialTile, _destinationTile);
            }
            else if (_destinationTile.IsWorkPlace)
            {
                _dropOnWorkPlace?.Invoke(_initialTile, _destinationTile);
            }
        }
        else
        {
            PlaceUnitOnTile(_draggedObject, _initialTile);
        }

        _draggedObject = null;
    }
}