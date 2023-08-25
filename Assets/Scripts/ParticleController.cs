using UnityEngine;

public class ParticleController : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystemCloud;

    public void Play()
    {
       _particleSystemCloud.Play(); 
    }

    public void Stop()
    {
        _particleSystemCloud.Stop(); 
    }
}
