using UnityEngine;

public class Tile : MonoBehaviour
{
    [field:SerializeField]
    public bool IsMergePlace { get; private set; }
    [field:SerializeField]
    public bool IsWorkPlace { get; private set; }
    
    public bool IsAvailable { get; private set; } = true;
    public Unit Unit { get; private set; }

    public void SetUnit(Unit unit)
    {
        Unit = unit;
        ChangeState(false);
    }

    public void ClearFromUnit()
    {
        Unit = null;
        ChangeState(true);
    }
    
    private void ChangeState(bool isAvailable)
    {
        IsAvailable = isAvailable;
    }
}