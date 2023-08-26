using UnityEngine;

public class Unit : MonoBehaviour, IDraggable
{
    public int Level { get; private set; }
    public int Salary { get; private set; }
    public bool IsWorking { get; private set; }
    
    [SerializeField]
    private SkinnedMeshRenderer _renderer;

    public void Initialize(UnitSettings unitSettings)
    {
        Level = unitSettings.Level;
        Salary = unitSettings.Salary;
        _renderer.material.color = unitSettings.Color;
    }

    public void ChangeWorkingState(bool isWorking)
    {
        IsWorking = isWorking;
    }
    
    public void Drag(Vector3 targetPosition)
    {
        transform.position = targetPosition;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
}