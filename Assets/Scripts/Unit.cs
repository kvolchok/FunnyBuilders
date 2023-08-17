using UnityEngine;

public class Unit : MonoBehaviour
{
    public int Level { get; private set; }
    
    private MeshRenderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
    }

    public void Initialize(UnitSettings unitSettings)
    {
        Level = unitSettings.Level;
        _renderer.material.color = unitSettings.Color;
    }
}