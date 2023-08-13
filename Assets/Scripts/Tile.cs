using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool IsAvailable { get; private set; } = true;
    
    public Unit Unit { get; private set; }

    public void ChangeState(bool isAvailable)
    {
        IsAvailable = isAvailable;
    }

    public void SetUnit(Unit unit)
    {
        Unit = unit;
        ChangeState(false);
    }
}