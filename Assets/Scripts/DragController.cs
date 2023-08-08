using UnityEngine;

public class DragController : MonoBehaviour
{
    public LayerMask Mask;
    [SerializeField] private float _distance;
    [SerializeField] private Collider _currentCollider;
    private Camera _camera;
    private Vector3 _offset;
    private Plane _dragPlane;


    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            SelectPart();
        }

        if (Input.GetMouseButtonUp(0))
        {
            Drop();
        }

        DragAndDropObject();
    }

    private void SelectPart()
    {
        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hitInfo, _distance, Mask))
        {
            _currentCollider = hitInfo.collider;
            _dragPlane = new Plane(_camera.transform.forward, _currentCollider.transform.position);
            float planeDist;
            _dragPlane.Raycast(ray, out planeDist);
            _offset = _currentCollider.transform.position - ray.GetPoint(planeDist);
        }
    }

    private void DragAndDropObject()
    {
        if (_currentCollider == null)
        {
            return;
        }

        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        float planeDist;
        _dragPlane.Raycast(ray, out planeDist);

        _currentCollider.transform.position = ray.GetPoint(planeDist) + _offset;

        if (_currentCollider.transform.position.y < 0.5f)
        {
            _currentCollider.transform.position = new Vector3(_currentCollider.transform.position.x, 0.5f,
                _currentCollider.transform.position.z);
        }
    }

    private void Drop()
    {
        if (_currentCollider == null)
        {
            return;
        }

        _currentCollider.transform.position = new Vector3(_currentCollider.transform.position.x, 0.5f,
            _currentCollider.transform.position.z);
        _currentCollider = null;
    }
}