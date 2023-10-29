using System.Collections.Generic;
using System.Threading;
using App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.ProjectileBehaviour.Shooter.ShootPolicy;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class SamuraiController : MonoBehaviour, ILooseGameListener
{
    [SerializeField]
    private SamuraiView _samuraiView;
    [SerializeField]
    private CountDownView _countDownView;
    
    private WavesSpawnPolicy _wavesSpawnPolicy;
    private BonusesConfig _bonusesConfig;
    private TokenController _tokenController;
    private CancellationToken _currentToken;
    private ActiveProjectileTypesContainer _activeProjectileTypesContainer;
    private CountDownCounter _countDownCounter;
    private HealthSystem _healthSystem;


    public void Construct(WavesSpawnPolicy wavesSpawnPolicy, BonusesConfig bonusesConfig
        , ActiveProjectileTypesContainer activeProjectileTypesContainer, HealthSystem healthSystem)
    {
        _healthSystem = healthSystem;
        _activeProjectileTypesContainer = activeProjectileTypesContainer;
        _countDownCounter = new CountDownCounter(_countDownView);
        _tokenController = new TokenController();
        _bonusesConfig = bonusesConfig;
        _wavesSpawnPolicy = wavesSpawnPolicy;
    }

    public void OnLooseGame()
    {
        _tokenController.CancelTokens();
        _samuraiView.Hide();
    }

    public async UniTaskVoid StartSamurai()
    {
        
        if(_currentToken != default)
            _tokenController.CancelToken(_currentToken);
        
        _currentToken = _tokenController.CreateCancellationToken();
        _samuraiView.Show();
        _activeProjectileTypesContainer.SetLooseHealthProjectiles(new List<ProjectileType>());
        _activeProjectileTypesContainer.SetActiveProjectiles(new List<ProjectileType>() { ProjectileType.Fruit });
        _wavesSpawnPolicy.SetIncreasedValues(_bonusesConfig.FruitsIncreaseValue, _bonusesConfig.PackDeltaTimeDecreaseValue);
        _countDownCounter.StartCountdown(_bonusesConfig.SamuraiTime);
        _healthSystem.SetImmortal();
        bool canceled = await UniTask.Delay((int)(1000*_bonusesConfig.SamuraiTime), DelayType.DeltaTime, PlayerLoopTiming.Update
            , _currentToken).SuppressCancellationThrow();
        _currentToken = default;
        if(canceled)
            return;
        _wavesSpawnPolicy.ResetGroupNumber();
        _wavesSpawnPolicy.ResetIncreasedValues();
        _activeProjectileTypesContainer.ResetActiveProjectiles();
        _activeProjectileTypesContainer.ResetLooseHealthProjectiles();
        _samuraiView.Hide();
        _healthSystem.SetNonImmortal();
    }
}