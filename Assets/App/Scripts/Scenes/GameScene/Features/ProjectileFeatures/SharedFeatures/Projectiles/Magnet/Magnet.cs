using App.Scripts.Scenes.GameScene.Features.ParticleFeatures;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Magnet : MonoBehaviour, ISlicable
{
    private ParticleSystemPlayer _particleSystemPlayer;
    private BonusesConfig _bonusesConfig;
    private TokenController _tokenController;

    public void Construct(ParticleSystemPlayer particleSystemPlayer, BonusesConfig bonusesConfig)
    {
        _tokenController = new TokenController();
        _bonusesConfig = bonusesConfig;
        _particleSystemPlayer = particleSystemPlayer;
    }

    private void OnDestroy()
    {
        _tokenController.CancelTokens();
    }

    public async void OnSlice()
    {
        GameObject particles = _particleSystemPlayer.PlayMagnetSliceParticlesTime(transform.position);
        FindObjectOfType<MagnetSuction>().StartSuction(transform.position, _bonusesConfig.MagnetTime, particles);
    }
}
