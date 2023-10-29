using App.Scripts.Scenes.GameScene.Features.ParticleFeatures;
using UnityEngine;

public class Ice : MonoBehaviour, ISlicable
{
    private ParticleSystemPlayer _particleSystemPlayer;
    private FreezeController _freezeController;
    private BonusesConfig _bonusesConfig;

    public void Construct(ParticleSystemPlayer particleSystemPlayer, FreezeController freezeController, BonusesConfig bonusesConfig)
    {
        _bonusesConfig = bonusesConfig;
        _freezeController = freezeController;
        _particleSystemPlayer = particleSystemPlayer;
    }
    
    public void OnSlice()
    {
       // _particleSystemPlayer.PlayIceParticles(transform.position);
       _freezeController.SetFrozeEffect(_bonusesConfig.FrozenTime);
    }
}
