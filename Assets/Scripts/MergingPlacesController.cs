using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MergingPlacesController : MonoBehaviour
{
    [field:SerializeField]
    public List<MergingPlace> MergingPlaces { get; private set; }
    
    public MergingPlace GetFirstAvailable()
    {
        return MergingPlaces.FirstOrDefault(mergingPlace => mergingPlace.IsAvailable);
    }
}