using UnityEngine;
using DG.Tweening;

public class DragController : MonoBehaviour
{
   // [SerializeField] private GridTile _gridTile;
    [SerializeField] private LayerMask _mask;
    [SerializeField] private int _distance;
    [SerializeField] private float _snapRange = 0.3f;
    private Collider _currentCollider;
    private Vector3 _startPositionObject;
    private Vector3 _initialPosition;


    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Drag();
        }

        if (Input.GetMouseButtonUp(0))
        {
            Drop();
        }
    }

    private void Drag()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hitInfo, _distance, _mask))
        {
            if (_currentCollider == null)
            {
                _currentCollider = hitInfo.collider;
                _startPositionObject = _currentCollider.transform.position;
                _initialPosition = _startPositionObject;
            }
        }

        var groundPlane = new Plane(Vector3.up, new Vector3(0, 0.3f, 0));
        if (groundPlane.Raycast(ray, out var position))
        {
            var worldPosition = ray.GetPoint(position);
            if (_currentCollider != null)
            {
                _currentCollider.transform.position = worldPosition;
            }
        }
    }


    private void Drop()
    {
        if (_currentCollider == null)
        {
            return;
        }

        if (_currentCollider.transform.position != _initialPosition)
        {
           // if (!IsSnapPointReached())
           // {
                _currentCollider.transform.DOMove(_initialPosition, 1.0f);
           // }
        }

        _currentCollider = null;
    }

   // private bool IsSnapPointReached()
   // {
   //     foreach (var item in _gridTile.Items)
   //     {
   //        
   //         var currentDistance = Vector3.Distance(_currentCollider.transform.position, item.transform.position);
//
   //         if (currentDistance <= _snapRange)
   //         {
   //             if (item != _currentCollider.transform )
   //             {
   //                 _currentCollider.gameObject.layer = default;
   //                 _gridTile.Items.Remove(item);
   //                 return true;
   //             }
   //         }
   //     }
//
   //     return false;
   // }
}