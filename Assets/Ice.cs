using App.Scripts.Scenes.GameScene.Features.ParticleFeatures;
using UnityEngine;

public class Ice : MonoBehaviour, ISlicable
{
    private ParticleSystemPlayer _particleSystemPlayer;
    private Freezer _freezer;
    private BonusesConfig _bonusesConfig;

    public void Construct(ParticleSystemPlayer particleSystemPlayer, Freezer freezer, BonusesConfig bonusesConfig)
    {
        _bonusesConfig = bonusesConfig;
        _freezer = freezer;
        _particleSystemPlayer = particleSystemPlayer;
    }
    
    public void OnSlice()
    {
       // _particleSystemPlayer.PlayIceParticles(transform.position);
       _freezer.SetFrozeEffect(_bonusesConfig.FrozenTime);
    }
}
