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
        private readonly GravitationConfig _gravitationConfig;
        private readonly SpawnConfig _spawnConfig;
        private readonly ProjectileConfig _projectileConfig;
        private readonly ShadowConfig _shadowConfig;
        private readonly FruitConfig _fruitConfig;

        public Shooter(IProjectileFactory projectileFactory, PhysicalFlightCalculator physicalFlightCalculator, SpawnAreasContainer spawnAreasContainer, IScreenSettingsProvider screenSettingsProvider
            , ProjectileConfig projectileConfig, ShadowConfig shadowConfig, FruitConfig fruitConfig, GravitationConfig gravitationConfig, SpawnConfig spawnConfig)
        {
            _projectileFactory = projectileFactory;
            _physicalFlightCalculator = physicalFlightCalculator;
            _spawnAreasContainer = spawnAreasContainer;
            _screenSettingsProvider = screenSettingsProvider;
            _projectileConfig = projectileConfig;
            _shadowConfig = shadowConfig;
            _fruitConfig = fruitConfig;
            _gravitationConfig = gravitationConfig;
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
            GetRandomTypeScaleAndPosition(areaData, _spawnConfig,_fruitConfig ,out var position, out var type, out var scale);

            int n = Random.Range(0, 5);
            ShootObject shootObject;
            Shadow shadow;
            if (n < 1)
            {
                shootObject = _projectileFactory.CreateFruit(type, position, scale, scale, out shadow).GetComponent<ShootObject>();
            }
            else if (n < 2)
            {
                shootObject = _projectileFactory.CreateHeart(position, scale, scale, out shadow).GetComponent<ShootObject>();
            }
            else
            {
                shootObject = _projectileFactory.CreateBomb(position, scale, scale, out shadow).GetComponent<ShootObject>();
            }

            Vector2 finalScale = GetLongestScale(scale, _fruitConfig.FruitScaleRange);

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
            float torqueValue = _projectileConfig.TorqueVelocityRange.GetRandomFloatBetween();
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

        private void GetRandomTypeScaleAndPosition(SpawnAreaData areaData, SpawnConfig spawnConfig, FruitConfig fruitConfig, out Vector2 position, out FruitType type, out Vector2 scale)
        {
            position = _screenSettingsProvider
                .ViewportToWorldPosition((areaData.ViewportLeftPosition, areaData.ViewportRightPosition)
                    .GetRandomPointBetween());
            type = spawnConfig.FruitTypes.GetRandomItem();
            float randomScaleValue = fruitConfig.FruitScaleRange.GetRandomBound();
            scale = new Vector2(randomScaleValue, randomScaleValue);
        }

        private Vector2 GetRandomMovementVector(SpawnAreaData areaData, float angle)
        {
            Vector2 moveVector = new Vector2(1, 1);
            moveVector.x *= Mathf.Cos(Mathf.Deg2Rad * (areaData.LineAngle + angle));
            moveVector.y *= Mathf.Sin(Mathf.Deg2Rad * (areaData.LineAngle + angle));
            moveVector *= _projectileConfig.ShootVelocityRange.GetRandomFloatBetween();
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