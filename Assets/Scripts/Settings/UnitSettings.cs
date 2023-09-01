using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "UnitSettings", menuName = "ScriptableObject/UnitSettings", order = 50)]
    public class UnitSettings : ScriptableObject
    {
        [field:SerializeField]
        public int Level { get; private set; }
        [field:SerializeField]
        public int Salary { get; private set; }
        [field:SerializeField]
        public Color Color { get; private set; }
    }
}