using App.Scripts.Scenes.GameScene.Features.ParticleFeatures;
using App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.ProjectileBehaviour.ShootSystem;
using UnityEngine;

public class Samurai : MonoBehaviour, ISlicable
{
    private SamuraiController _samuraiController;
    private ShootSystem _shootSystem;
    private ParticleSystemPlayer _particleSystemPlayer;

    public void Construct(SamuraiController samuraiController, ParticleSystemPlayer particleSystemPlayer)
    {
        _particleSystemPlayer = particleSystemPlayer;
        _samuraiController = samuraiController;
    }
    
    public void OnSlice()
    {
        _particleSystemPlayer.PlaySamuraiParticles(transform);
        _samuraiController.StartSamurai();
    }
}
