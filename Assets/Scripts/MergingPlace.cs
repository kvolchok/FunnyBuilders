using UnityEngine;

public class MergingPlace : UnitHolder
{
    [SerializeField]
    private ParticleSystem _particleAppear;

    public void ShowAppearAnimation()
    {
        _particleAppear.Play();
    }
}