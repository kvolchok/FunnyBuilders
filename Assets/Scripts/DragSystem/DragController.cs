using System;
using UnityEngine;
using UnityEngine.Events;

namespace DragSystem
{
    public class DragController : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent<Unit, MergingPlace> _dropOnMergingPlace;
        [SerializeField]
        private UnityEvent<Unit, WorkPlace> _dropOnWorkPlace;

        public event Func<IDraggable, bool> CanTakeObject;
        public event Action<Unit, DropPlace> OnCantDropObject;
    
        public DropPlace InitialDropPlace { get; private set; }

        [SerializeField]
        private LayerMask _planeLayerMask;
    
        private Vector3 _unitOffset;
        private Camera _camera;
    
        private IDraggable _draggedObject;
        private DropPlace _destinationDropPlace;
        private bool _canDropObject;

        public void Initialize(Vector3 unitOffset)
        {
            _unitOffset = unitOffset;
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
            if (!CanTakeObject.Invoke(draggedObject))
            {
                return;   
            }

            var initialUnitHolder = GetHolderUnderDraggedObject(draggedObject);
            if (initialUnitHolder == null)
            {
                return;
            }

            _draggedObject = draggedObject;
            InitialDropPlace = initialUnitHolder;
        }

        private DropPlace GetHolderUnderDraggedObject(IDraggable draggedObject)
        {
            if (!Physics.Raycast(draggedObject.Transform.position, Vector3.down, out var hit,
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
                var dragPoint = hit.point + _unitOffset;
                _draggedObject.Drag(dragPoint);
                _canDropObject = CanDropObject(_draggedObject.Transform.position);
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
        
            InitialDropPlace.Clear();

            if (_canDropObject && _draggedObject is Unit unit)
            {
                if (_destinationDropPlace is MergingPlace mergingPlace)
                {
                    _dropOnMergingPlace?.Invoke(unit, mergingPlace);
                }
                else if (_destinationDropPlace is WorkPlace workPlace)
                {
                    _dropOnWorkPlace?.Invoke(unit, workPlace);
                }
            }
            else
            {
                unit = _draggedObject as Unit;
                OnCantDropObject?.Invoke(unit, InitialDropPlace);
            }
        
            _draggedObject = null;
        }
    }
}