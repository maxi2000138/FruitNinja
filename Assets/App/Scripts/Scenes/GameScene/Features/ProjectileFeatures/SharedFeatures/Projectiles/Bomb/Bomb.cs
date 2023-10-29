using App.Scripts.Scenes.GameScene.Features.ParticleFeatures;
using UnityEngine;

public class Bomb : MonoBehaviour, ISlicable
{
        
    private ParticleSystemPlayer _particleSystemPlayer;
    private HealthSystem _healthSystem;

    public void Construct(ParticleSystemPlayer particleSystemPlayer, HealthSystem healthSystem)
    {
        _healthSystem = healthSystem;
        _particleSystemPlayer = particleSystemPlayer;
    }
    
    public void OnSlice()
    {
        _healthSystem.LooseLife();
        _particleSystemPlayer.PlayBombSliceParticles(transform.position);
    }
}
