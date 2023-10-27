using App.Scripts.ListExtensions;
using App.Scripts.Scenes.GameScene.Configs;
using App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ColliderFeatures;
using App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ForcesTypes.Mover;
using App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.ProjectileBehaviour.ProjectileFactory;
using App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.ProjectileBehaviour.Shooter.ProjectileShooter;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class StringBag : MonoBehaviour, ISlicable
{
    [SerializeField]
    private Mover _mover;
    
    private BonusesConfig _bonusesConfig;
    private Shooter _shooter;
    private ProjectileFactory _projectileFactory;
    private ProjectileConfig _projectileConfig;
    private SpawnConfig _spawnConfig;
    private TokenController _tokenController;

    public void Construct(Shooter shooter, ProjectileFactory projectileFactory, ProjectileConfig projectileConfig
        , SpawnConfig spawnConfig, BonusesConfig bonusesConfig)
    {
        _tokenController = new TokenController();
        _spawnConfig = spawnConfig;
        _projectileConfig = projectileConfig;
        _projectileFactory = projectileFactory;
        _bonusesConfig = bonusesConfig;
        _shooter = shooter;
    }
    
    public void OnSlice()
    {
        for (int i = 0; i < _bonusesConfig.StringBagFruitsAmount; i++)
        {
            _projectileFactory.GetRandomScaleInConfigRange(ProjectileType.Fruit, _projectileConfig, out var scale);
            ProjectileObject projectileObject = _projectileFactory.SpawnProjectileByType(ProjectileType.Fruit, transform.position, scale, out var shadow);
            SetStartInvisible(projectileObject.GetComponent<SliceCircleCollider>());
            _shooter.SetScalingAndShootByAngle(ProjectileType.Fruit, projectileObject.GetComponent<ShootObject>(), shadow, transform.position, scale, GetMovementVector()* _bonusesConfig.StringBagFruitForce);
        }
    }

    private async UniTaskVoid SetStartInvisible(SliceCircleCollider sliceCircleCollider)
    {
        sliceCircleCollider.Disable();
        await UniTask.Delay((int)(1000 * _bonusesConfig.StringBagInivisibleTime), DelayType.DeltaTime,
            PlayerLoopTiming.Update, _tokenController.CreateCancellationToken()).SuppressCancellationThrow();
        sliceCircleCollider.Enable();
    }

    private Vector2 GetMovementVector()
    {
        return Quaternion.AngleAxis(
                   (-_bonusesConfig.StringBagFruitsAngleRange, _bonusesConfig.StringBagFruitsAngleRange)
                   .GetRandomFloatBetween(), Vector3.forward) * Vector3.up;
    }
}
