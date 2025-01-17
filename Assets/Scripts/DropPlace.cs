using UnityEngine;

public abstract class DropPlace : MonoBehaviour
{
    public bool IsAvailable { get; private set; } = true;
    public Unit Unit { get; private set; }

    public void SetUnit(Unit unit)
    {
        Unit = unit;
        ChangeState(false);
    }

    public void Clear()
    {
        Unit = null;
        ChangeState(true);
    }
    
    private void ChangeState(bool isAvailable)
    {
        IsAvailable = isAvailable;
    }
}