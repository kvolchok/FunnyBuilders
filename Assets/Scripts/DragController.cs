using UnityEngine;
using DG.Tweening;

public class DragController : MonoBehaviour
{
    [SerializeField] private LayerMask _raycastLayerMask;
    [SerializeField] private float _movementDuration = 1f;
    [SerializeField] private Vector3 _offset;
    private Collider _draggedObject;
    private Vector3 _startPositionObject;
    private Vector3 _dropSpotPosition;
    private Camera _camera;
    private bool _isOverDropSpot;

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
            if (hit.collider.CompareTag("Player"))
            {
                _draggedObject = hit.collider;
                _startPositionObject = _draggedObject.transform.position;
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
                _draggedObject.transform.position = new Vector3(hit.point.x, hit.point.y, hit.point.z) + _offset;

                CheckDropSpot();
            }
        }
    }

    private void DropObject()
    {
        if (_draggedObject == null)
        {
            return;
        }

        if (_isOverDropSpot)
        {
            _draggedObject.transform.position = _dropSpotPosition + _offset;
        }
        else
        {
            _draggedObject.transform.DOMove(_startPositionObject, _movementDuration);
        }


        _draggedObject = null;
    }

    private void CheckDropSpot()
    {
        var ray = new Ray(_draggedObject.transform.position, Vector3.down);
        if (Physics.Raycast(ray, out var hitInfo))
        {
            var hit = hitInfo.collider.gameObject;
            var spot = hit.CompareTag("Finish");
            if (spot)
            {
                _isOverDropSpot = true;
                _dropSpotPosition = hit.transform.position;
            }
        }
    }
}