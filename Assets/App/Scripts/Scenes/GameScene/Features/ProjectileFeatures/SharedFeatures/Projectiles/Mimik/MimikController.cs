using App.Scripts.ListExtensions;
using App.Scripts.Scenes.GameScene.Configs;
using App.Scripts.Scenes.GameScene.Features.ParticleFeatures;
using App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.ProjectileBehaviour.ProjectileDestroyer.DestroyTrigger;
using App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.ProjectileBehaviour.ProjectileFactory;
using App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.ProjectileBehaviour.Shooter.ProjectileShooter;
using App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.ShadowFeatures;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class MimikController
{
    private ProjectileObject _projectileObject;
    private TokenController _tokenController;
    private BonusesConfig _bonusesConfig;
    private readonly ProjectileFactory _projectileFactory;
    private readonly IShooter _shooter;
    private readonly IDestroyTrigger _destroyTrigger;
    private readonly SpawnConfig _spawnConfig;
    private readonly ProjectileConfig _projectileConfig;
    private readonly ParticleSystemPlayer _particleSystemPlayer;

    public MimikController(ProjectileFactory projectileFactory, IShooter shooter, IDestroyTrigger destroyTrigger
        , ProjectileObject startProjectileObject,BonusesConfig bonusesConfig, SpawnConfig spawnConfig
        , ProjectileConfig projectileConfig, ParticleSystemPlayer particleSystemPlayer)
    {
        _tokenController = new TokenController();
        _bonusesConfig = bonusesConfig;
        _spawnConfig = spawnConfig;
        _projectileConfig = projectileConfig;
        _particleSystemPlayer = particleSystemPlayer;
        _projectileFactory = projectileFactory;
        _shooter = shooter;
        _destroyTrigger = destroyTrigger;
        _projectileObject = startProjectileObject;
    }

    public void StartMimikBehaviour()
    {
        _particleSystemPlayer.PlayMimikDefaultAndParent(_projectileObject.transform);
        ChangingAsyncMethod();
        _projectileObject.OnDestroyEvent += StopMimikBehavior;
    }
    
    public void StopMimikBehavior()
    {
        _tokenController.CancelTokens();
    }

    public async UniTaskVoid ChangingAsyncMethod()
    {
        while (true)
        {
            bool isCanceled = await UniTask.Delay((int)((_bonusesConfig.MimikChangeTime - _bonusesConfig.MimikBeforeChangeParticleDeltaTime) * 1000)
                , DelayType.DeltaTime, PlayerLoopTiming.Update,
                _tokenController.CreateCancellationToken()).SuppressCancellationThrow();
            if(isCanceled) return;
            
            _particleSystemPlayer.PlayMimikBeforeChangeAndParent(_projectileObject.transform);
            
            isCanceled = await UniTask.Delay((int)((_bonusesConfig.MimikBeforeChangeParticleDeltaTime) * 1000), DelayType.DeltaTime, PlayerLoopTiming.Update,
                _tokenController.CreateCancellationToken()).SuppressCancellationThrow();
            if(isCanceled) return;

            ChangeProjectile();
            _particleSystemPlayer.PlayMimikAfterChange(_projectileObject.transform);
            _particleSystemPlayer.PlayMimikDefaultAndParent(_projectileObject.transform);
        }
    }
    
    public void ChangeProjectile()
    {
        ProjectileType type = _spawnConfig.ActiveProjectileTypes.GetRandomItem();
        _projectileFactory.GetRandomScaleInConfigRange(type, _projectileConfig, out Vector2 scale);
        Vector2 position = _projectileObject.transform.position;
        ProjectileObject projectileObject = _projectileFactory.SpawnProjectileByType(type, position, scale, out Shadow shadow);
        projectileObject.OnDestroyEvent += StopMimikBehavior;
        _projectileObject.OnDestroyEvent -= StopMimikBehavior;
        _shooter.SetScalingAndShootByAngle(type, projectileObject.GetComponent<ShootObject>(), shadow, position, scale, _projectileObject.Mover.MovementVector);
        _destroyTrigger.TriggerGroup(_projectileObject, true);
        _projectileObject = projectileObject;
    }
}
