using App.Scripts.Scenes.GameScene.Features.ParticleFeatures;
using UnityEngine;

public class Ice : MonoBehaviour, ISlicable
{
    private ParticleSystemPlayer _particleSystemPlayer;
    private FrozerService _frozerService;
    private BonusesConfig _bonusesConfig;

    public void Construct(ParticleSystemPlayer particleSystemPlayer, FrozerService frozerService, BonusesConfig bonusesConfig)
    {
        _bonusesConfig = bonusesConfig;
        _frozerService = frozerService;
        _particleSystemPlayer = particleSystemPlayer;
    }
    
    public void OnSlice()
    {
       // _particleSystemPlayer.PlayIceParticles(transform.position);
       _frozerService.SetFrozeEffect(_bonusesConfig.FrozenTime);
    }
}
