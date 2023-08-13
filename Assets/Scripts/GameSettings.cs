using System;
using UnityEngine;

[Serializable]
public class GameSettings
{
    [field:SerializeField]
    public UnitSettings[] UnitSettings { get; private set; } 
}