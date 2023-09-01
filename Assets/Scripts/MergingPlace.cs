using UnityEngine;

public class MergingPlace : DropPlace
{
    [SerializeField]
    private ParticleSystem _particleFire;

    public void ShowMergeAnimation()
    {
        _particleFire.Play();
    }
}