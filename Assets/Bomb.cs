using App.Scripts.Scenes.GameScene.Features.ParticleFeatures;
using UnityEngine;

public class Bomb : MonoBehaviour, ISlicable
{
        
    private ParticleSystemPlayer _particleSystemPlayer;

    public void Construct(ParticleSystemPlayer particleSystemPlayer)
    {
        _particleSystemPlayer = particleSystemPlayer;
    }
    
    public void OnSlice()
    {
        _particleSystemPlayer.PlayBombSliceParticles(transform.position);
    }
}
