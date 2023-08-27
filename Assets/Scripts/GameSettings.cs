using System;
using UnityEngine;

[Serializable]
public class GameSettings
{
    [field:SerializeField]
    public int StartMoney { get; private set; }
    
    [field:SerializeField]
    public UnitSettings[] UnitSettings { get; private set; }
    [field:SerializeField]
    public int[] UnitPrices { get; private set; } 
    [field:SerializeField]
    public Vector3 UnitOffset { get; private set; }
    [field:SerializeField]
    public float UnitMovementDuration { get; private set; }
    [field:SerializeField]
    public float UnitPaymentInterval { get; private set; }
    
    [field:SerializeField]
    public int[] SpotPrices { get; private set; } 
    [field:SerializeField]
    public int AmountAvailableSpots { get; private set; }
    
    [field:SerializeField]
    public int BuildingConstructionCost { get; private set; }
    [field:SerializeField]
    public float DurationBuildingHeight { get; private set; }
}