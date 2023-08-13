using UnityEngine;

[CreateAssetMenu(fileName = "UnitSettings", menuName = "ScriptableObject/UnitSettings", order = 50)]
public class UnitSettings : ScriptableObject
{
    [field:SerializeField]
    public int Level { get; private set; }
    [field:SerializeField]
    public Color Color { get; private set; }
}