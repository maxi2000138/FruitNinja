using App.Scripts.Scenes.GameScene.Features.ParticleFeatures;
using UnityEngine;

public class Heart : MonoBehaviour, ISlicable
{
    private ParticleSystemPlayer _particleSystemPlayer;

    public void Construct(ParticleSystemPlayer particleSystemPlayer)
    {
        _particleSystemPlayer = particleSystemPlayer;
    }
    
    public void OnSlice()
    {
        _particleSystemPlayer.PlayHeartParticles(transform.position);
    }
}
