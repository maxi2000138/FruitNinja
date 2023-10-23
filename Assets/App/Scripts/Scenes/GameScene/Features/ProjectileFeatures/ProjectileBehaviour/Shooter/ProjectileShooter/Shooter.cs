using App.Scripts.ListExtensions;
using App.Scripts.Scenes.GameScene.Configs;
using App.Scripts.Scenes.GameScene.Features.CameraFeatures.ScreenSettingsProvider;
using App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ForcesApplier;
using App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.FruitFeatures.Enum;
using App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.FruitFeatures.Fruit;
using App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.ProjectileBehaviour.ProjectileFactory;
using App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.ShadowFeatures;
using App.Scripts.Scenes.GameScene.Features.SpawnAreaFeatures;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.ProjectileBehaviour.Shooter.ProjectileShooter
{
    public class Shooter : IShooter
    {
        private float _highestYValue;
        private readonly IScreenSettingsProvider _screenSettingsProvider;
        private readonly SpawnAreasContainer _spawnAreasContainer;
        private readonly IProjectileFactory _projectileFactory;
        private readonly PhysicalFlightCalculator _physicalFlightCalculator;
        private readonly PhysicsConfig _physicsConfig;
        private readonly SpawnConfig _spawnConfig;
        private readonly ShootConfig _shootConfig;
        private readonly ShadowConfig _shadowConfig;
        private readonly ProjectileConfig _projectileConfig;

        public Shooter(IProjectileFactory projectileFactory, PhysicalFlightCalculator physicalFlightCalculator, SpawnAreasContainer spawnAreasContainer, IScreenSettingsProvider screenSettingsProvider
            , ShootConfig shootConfig, ShadowConfig shadowConfig, ProjectileConfig projectileConfig, PhysicsConfig physicsConfig, SpawnConfig spawnConfig)
        {
            _projectileFactory = projectileFactory;
            _physicalFlightCalculator = physicalFlightCalculator;
            _spawnAreasContainer = spawnAreasContainer;
            _screenSettingsProvider = screenSettingsProvider;
            _shootConfig = shootConfig;
            _shadowConfig = shadowConfig;
            _projectileConfig = projectileConfig;
            _physicsConfig = physicsConfig;
            _spawnConfig = spawnConfig;
        }
        
        public void Shoot()
        {
            SpawnAreaData areaData = _spawnAreasContainer.SpawnAreaHandlers.GetRandomItemByProbability(data => data.Probability);
            float angle = (areaData.ShootMinAngle, areaData.ShootMaxAngle).GetRandomFloatBetween();
            SpawnProjectileAndShootByAngle(areaData, angle);
        }

        private void SpawnProjectileAndShootByAngle(SpawnAreaData areaData, float angle)
        {

            ShootObject shootObject = null;
            Shadow shadow = null;
            var (projectileType, value) = _spawnConfig.ProjectileSpawnProbability.GetRandomItemByProbability(data => data.Value);
            GetRandomScaleAndPosition(areaData, projectileType,_projectileConfig ,out var position, out var scale);

            switch (projectileType)
            {
                case(ProjectileType.Fruit):
                    var type = _spawnConfig.ActiveFruitTypes.GetRandomItem();
                    shootObject = _projectileFactory.CreateFruit(type, position, scale, scale, out shadow).GetComponent<ShootObject>();
                    break;
                case(ProjectileType.Bomb):
                    shootObject = _projectileFactory.CreateBomb(position, scale, scale, out shadow).GetComponent<ShootObject>();
                    break;                    
                case(ProjectileType.Heart):
                    shootObject = _projectileFactory.CreateHeart(position, scale, scale, out shadow).GetComponent<ShootObject>();
                    break;                    
                    
            }
            
            Vector2 finalScale = GetLongestScale(scale, _projectileConfig.ProjectileScales[projectileType].Scale);


            float flyTime = _physicalFlightCalculator.GetFlyTimeFromYPosition(position.y);
            shootObject.ScaleByTimeApplier.StartScaling(scale,finalScale, flyTime);
            
            SetShadowOffseting(shadow,finalScale/scale, flyTime);
            SetShadowScaling(shadow, finalScale/scale, flyTime);
            
            RotateProjectile(shootObject);
            
            ShootProjectile(areaData, angle, shootObject, 1f);
        }

        private void ShootProjectile(SpawnAreaData areaData, float angle, ShootObject shootObject, float maxScale)
        {
            Vector2 moveVector = GetRandomMovementVector(areaData, angle);
            moveVector = _physicalFlightCalculator.ConstrainSpeed(FruitSpriteHeight(shootObject, maxScale), moveVector);
            ShootProjectile(shootObject, moveVector);
        }
        
        
        private void RotateProjectile(ShootObject shootObject)
        {
            float torqueValue = _shootConfig.TorqueVelocityRange.GetRandomFloatBetween();
            shootObject.TorqueApplier.AddTorque(torqueValue);
        }
        
        private void SetShadowScaling(Shadow shadow, Vector2 scaleRatio, float flyTime)
        {
            Vector2 startOffset = shadow.transform.localScale;
            float scaleFactor = scaleRatio.x;
            scaleFactor *= _shadowConfig.ShadowScaleScaler;
            
            Vector2 finalOffset = Vector2.LerpUnclamped(Vector2.zero, startOffset, scaleFactor);

            shadow.ScaleByTimeApplier.StartScaling(startOffset, finalOffset, flyTime);
        }

        private void SetShadowOffseting(Shadow shadow, Vector2 scaleRatio, float flyTime)
        {
            Vector2 startOffset = shadow.Offset;
            float scaleFactor = scaleRatio.x;
            if (scaleFactor < 1)
                scaleFactor /= _shadowConfig.ShadowOffsetScaler;
            else 
                scaleFactor *= _shadowConfig.ShadowOffsetScaler;
            
            Vector2 finalOffset = Vector2.LerpUnclamped(Vector2.zero, startOffset, scaleFactor);

            shadow.OffsetByTimeApplier.StartOffseting(startOffset, finalOffset, flyTime);
        }

        private float FruitSpriteHeight(ShootObject shootObject, float maxScale)
        {
            return shootObject.transform.position.y + (shootObject.ProjectileObject.SpriteDiagonal()/2 * maxScale);
        }

        public void ShootProjectile(ShootObject shootObject, Vector2 moveVector)
        {
            VelocityApplier velocityApplier = shootObject.VelocityApplier;
            velocityApplier.AddVelocity(moveVector);
        }

        private void GetRandomScaleAndPosition(SpawnAreaData areaData, ProjectileType projectileType, ProjectileConfig projectileConfig, out Vector2 position, out Vector2 scale)
        {
            position = _screenSettingsProvider
                .ViewportToWorldPosition((areaData.ViewportLeftPosition, areaData.ViewportRightPosition)
                    .GetRandomPointBetween());
            float randomScaleValue = projectileConfig.ProjectileScales[projectileType].Scale.GetRandomBound();
            scale = new Vector2(randomScaleValue, randomScaleValue);
        }

        private Vector2 GetRandomMovementVector(SpawnAreaData areaData, float angle)
        {
            Vector2 moveVector = new Vector2(1, 1);
            moveVector.x *= Mathf.Cos(Mathf.Deg2Rad * (areaData.LineAngle + angle));
            moveVector.y *= Mathf.Sin(Mathf.Deg2Rad * (areaData.LineAngle + angle));
            moveVector *= _shootConfig.ShootVelocityRange.GetRandomFloatBetween();
            return moveVector;
        }
    

        private Vector2 GetLongestScale(Vector2 currentScale, Vector2 scaleRange)
        {
            float downScaleDistance = Mathf.Abs(scaleRange.x - currentScale.x) + Mathf.Abs(scaleRange.x - currentScale.y);
            float upScaleDistance = Mathf.Abs(scaleRange.y - currentScale.x) + Mathf.Abs(scaleRange.y - currentScale.y);
            return downScaleDistance > upScaleDistance ? new Vector2(scaleRange.x, scaleRange.x) : new Vector2(scaleRange.y, scaleRange.y);
        }
    }
}