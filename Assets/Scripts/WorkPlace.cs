using UnityEngine;

public class WorkPlace : UnitHolder
{
    [SerializeField]
    private ParticleSystem _particleSweat;
    
    public void StartSweat()
    {
        _particleSweat.Play();
    }
    
    public void StopSweat()
    {
        _particleSweat.Stop();
    }
}