using UnityEngine;

public class MergingPlace : UnitHolder
{
    [SerializeField]
    private ParticleSystem _particleFire;

    public void ShowMergeAnimation()
    {
        _particleFire.Play();
    }
}