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
        private readonly PhysicalFlightCalculator _physicalFlightCalculator;
        private readonly ShootConfig _shootConfig;
        private readonly ShadowConfig _shadowConfig;
        private readonly ProjectileConfig _projectileConfig;
        private const float _spriteHeightOffset = 2f;

        public Shooter(PhysicalFlightCalculator physicalFlightCalculator, ShootConfig shootConfig
            , ShadowConfig shadowConfig, ProjectileConfig projectileConfig)
        {
            _physicalFlightCalculator = physicalFlightCalculator;
            _shootConfig = shootConfig;
            _shadowConfig = shadowConfig;
            _projectileConfig = projectileConfig;
        }
        
        public void SetScalingAndShootByAngle(ProjectileType projectileType, ShootObject shootObject, Shadow shadow, Vector2 position, Vector2 scale, Vector2 moveVector)
        {
            Vector2 finalScale = GetLongestScale(scale, _projectileConfig.ProjectileScales[projectileType].Scale);


            float flyTime = _physicalFlightCalculator.GetFlyTimeFromYPosition(position.y);
            shootObject.ScaleByTimeApplier.StartScaling(scale, finalScale, flyTime);

            SetShadowOffseting(shadow, finalScale / scale, flyTime);
            SetShadowScaling(shadow, finalScale / scale, flyTime);

            RotateProjectile(shootObject);

            float maxScale = finalScale.x > scale.x ? finalScale.x : scale.x;
            ShootProjectileByVector(moveVector, shootObject, maxScale);
        }

        private void ShootProjectileByVector(Vector2 moveVector, ShootObject shootObject, float maxScale)
        {
            moveVector = _physicalFlightCalculator.ConstrainSpeed(FruitSpriteHeight(shootObject, maxScale), moveVector*50f);
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
            return shootObject.transform.position.y + _spriteHeightOffset;
        }

        private void ShootProjectile(ShootObject shootObject, Vector2 moveVector)
        {
            VelocityApplier velocityApplier = shootObject.VelocityApplier;
            velocityApplier.AddVelocity(moveVector);
        }


        private Vector2 GetLongestScale(Vector2 currentScale, Vector2 scaleRange)
        {
            float downScaleDistance = Mathf.Abs(scaleRange.x - currentScale.x) + Mathf.Abs(scaleRange.x - currentScale.y);
            float upScaleDistance = Mathf.Abs(scaleRange.y - currentScale.x) + Mathf.Abs(scaleRange.y - currentScale.y);
            return downScaleDistance > upScaleDistance ? new Vector2(scaleRange.x, scaleRange.x) : new Vector2(scaleRange.y, scaleRange.y);
        }
    }
}