using App.Scripts.ListExtensions;
using App.Scripts.Scenes.GameScene.Configs;
using App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.ProjectileBehaviour.ProjectileDestroyer.DestroyTrigger;
using App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.ProjectileBehaviour.ProjectileFactory;
using App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.ProjectileBehaviour.Shooter.ProjectileShooter;
using App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.SharedFeatures;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class MimikController : MonoBehaviour
{
    private ProjectileFactory _projectileFactory;
    private IFullSliceObject _currentProjectile;
    private DestroyTrigger _destroyTrigger;
    private BonusesConfig _bonusesConfig;
    private SpawnConfig _spawnConfig;
    private IShooter _shooter;

    public void Construct(ProjectileFactory projectileFactory, DestroyTrigger destroyTrigger, IShooter shooter, SpawnConfig spawnConfig, BonusesConfig bonusesConfig)
    {
        _bonusesConfig = bonusesConfig;
        _shooter = shooter;
        _spawnConfig = spawnConfig;
        _destroyTrigger = destroyTrigger;
        _projectileFactory = projectileFactory;
    }

    public async UniTaskVoid StartChanging()
    {
        while (true)
        {
            ChangeProjectile();
            await UniTask.Delay((int)(1000f*_bonusesConfig.ChangeTime), DelayType.DeltaTime).SuppressCancellationThrow();
        }
    }

    public void ChangeProjectile()
    {
        ProjectileType projectileType = _spawnConfig.ActiveProjectileTypes.GetRandomItem();
        ProjectileObject projectileObject = _projectileFactory.SpawnProjectileByType(projectileType, _currentProjectile.ProjectileObject.transform.position,
            _currentProjectile.ProjectileObject.Scale, out var shadow);
        
        _shooter.SetScalingAndShootByAngle(projectileType, projectileObject.GetComponent<ShootObject>(), shadow, _currentProjectile.ProjectileObject.transform.position
            ,_currentProjectile.ProjectileObject.Scale, _currentProjectile.ProjectileObject.Mover.MovementVector);
        
        _destroyTrigger.TriggerGroup(_currentProjectile.ProjectileObject);
        _currentProjectile = projectileObject.GetComponent<IFullSliceObject>();
    }
}
