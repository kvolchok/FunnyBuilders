using UnityEngine;

public class WorkPlace : UnitHolder
{
    [SerializeField] private ParticleSystem _particleTear;

    public void StartSweat()
    {
        _particleTear.Play();
    }   
    public void StopSweat()
    {
        _particleTear.Stop();
    }   
}