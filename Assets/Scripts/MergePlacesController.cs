using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MergePlacesController : MonoBehaviour
{
    [field:SerializeField]
    public List<Tile> Tiles { get; private set; }
    
    public Tile GetFirstAvailable()
    {
        return Tiles.FirstOrDefault(tile => tile.IsAvailable);
    }
}