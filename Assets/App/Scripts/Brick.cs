using App.Scripts.Scenes.GameScene.Features.InputFeatures;
using App.Scripts.Scenes.GameScene.Features.ParticleFeatures;
using UnityEngine;

public class Brick : MonoBehaviour, ISlicable
{
    private Slicer _slicer;
    private ParticleSystemPlayer _particleSystemPlayer;

    public void Construct(Slicer slicer, ParticleSystemPlayer particleSystemPlayer)
    {
        _particleSystemPlayer = particleSystemPlayer;
        _slicer = slicer;
    }
    
    public void OnSlice()
    {
        _particleSystemPlayer.PlayBrickParticles(transform.position);
        _slicer.EndSlicing();
    }
}
