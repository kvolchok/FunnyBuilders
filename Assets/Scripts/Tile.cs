using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool IsAvailable { get; private set; } = true;

    public void ChangeState(bool isAvailable)
    {
        IsAvailable = isAvailable;
    }
}