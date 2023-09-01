using UnityEngine;

public class WorkPlace : DropPlace
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