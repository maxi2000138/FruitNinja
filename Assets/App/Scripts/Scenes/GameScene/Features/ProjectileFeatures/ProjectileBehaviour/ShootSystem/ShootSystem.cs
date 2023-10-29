using System.Collections.Generic;
using App.Scripts.ListExtensions;
using App.Scripts.Scenes.GameScene.Configs;
using App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.ProjectileBehaviour.ProjectileFactory;
using App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.ProjectileBehaviour.Shooter.ProjectileShooter;
using App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.ProjectileBehaviour.Shooter.ShootPolicy;
using App.Scripts.Scenes.GameScene.Features.SpawnAreaFeatures;
using App.Scripts.Scenes.Infrastructure.MonoInterfaces;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.ProjectileBehaviour.ShootSystem
{
    public class ShootSystem : IInitializable, ILooseGameListener, IRestartGameListener
    {
        private readonly IShootPolicy _shootPolicy;
        private readonly IProjectileFactory _projectileFactory;
        private readonly SpawnAreasContainer _spawnAreasContainer;
        private readonly IShooter _shooter;
        private readonly SpawnConfig _spawnConfig;
        private readonly ProjectileConfig _projectileConfig;
        private readonly ShootConfig _shootConfig;
        private readonly ActiveProjectileTypesContainer _activeProjectileTypesContainer;

        public ShootSystem(IProjectileFactory projectileFactory,SpawnAreasContainer spawnAreasContainer, IShooter shooter
            , IShootPolicy shootPolicy, SpawnConfig spawnConfig, ProjectileConfig projectileConfig, ShootConfig shootConfig
            , ActiveProjectileTypesContainer activeProjectileTypesContainer)
        {
            _spawnAreasContainer = spawnAreasContainer;
            _projectileFactory = projectileFactory;
            _projectileConfig = projectileConfig;
            _shooter = shooter;
            _shootPolicy = shootPolicy;
            _shootConfig = shootConfig;
            _activeProjectileTypesContainer = activeProjectileTypesContainer;
            _spawnConfig = spawnConfig;
        }

        public void Initialize()
        {
            _activeProjectileTypesContainer.RestActiveProjectile();
            StartShooting();
        }

        public void OnRestartGame()
        {
            StartShooting();
        }

        public void OnLooseGame()
        {
            StopShooting();
        }

        public void StartShooting()
        {
            StopShooting();
            _shootPolicy.NeedShoot += SpawnAndShoot;
            _shootPolicy.StartWorking();
        }

        public void StopShooting()
        {
            _shootPolicy.StopWorking();
            _shootPolicy.NeedShoot -= SpawnAndShoot;
        }
        

        public void SpawnAndShoot()
        {
            var (projectileType,_) = _spawnConfig.ProjectileSpawnProbability.GetRandomItemByProbability(data =>
            {
                if (_activeProjectileTypesContainer.ProjectileTypes.Contains(data.Key))
                    return data.Value;
                
                return 0;
            });
            
            SpawnAreaData areaData = _spawnAreasContainer.SpawnAreaHandlers.GetRandomItemByProbability(data => data.Probability);
            float angle = (areaData.ShootMinAngle, areaData.ShootMaxAngle).GetRandomFloatBetween();
            Vector2 moveVector = GetRandomMovementVector(areaData, angle);
            _projectileFactory.GetRandomPositionInConfigRange(areaData,out var position);
            _projectileFactory.GetRandomScaleInConfigRange(projectileType, _projectileConfig, out var scale);
            ShootObject shootObject = _projectileFactory.SpawnProjectileByType(projectileType, position, scale, out var shadow).GetComponent<ShootObject>();
            _shooter.SetScalingAndShootByAngle(projectileType, shootObject, shadow, position, scale, moveVector);
        }
        
        private Vector2 GetRandomMovementVector(SpawnAreaData areaData, float angle)
        {
            Vector2 moveVector = new Vector2(1, 1);
            moveVector.x *= Mathf.Cos(Mathf.Deg2Rad * (areaData.LineAngle + angle));
            moveVector.y *= Mathf.Sin(Mathf.Deg2Rad * (areaData.LineAngle + angle));
            moveVector *= _shootConfig.ShootVelocityRange.GetRandomFloatBetween();
            return moveVector;
        }
    }
}
