using UnityEngine;

public class WorkPlace : UnitHolder
{
    [SerializeField]
    private ParticleSystem _particleSweat;
    
    public void TurnOnSweatAnimation()
    {
        _particleSweat.Play();
    }
    
    public void TurnOffSweatAnimation()
    {
        _particleSweat.Stop();
    }
}