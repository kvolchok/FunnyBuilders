using System;
using UnityEngine;

[Serializable]
public class GameSettings
{
    [field:SerializeField]
    public UnitSettings[] UnitSettings { get; private set; }
    [field:SerializeField]
    public int[] UnitPrices { get; private set; } 
    [field:SerializeField]
    public int StartMoney { get; private set; }
}