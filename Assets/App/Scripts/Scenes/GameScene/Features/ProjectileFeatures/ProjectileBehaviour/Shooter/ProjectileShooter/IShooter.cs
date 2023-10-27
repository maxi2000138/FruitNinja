using App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.ShadowFeatures;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.ProjectileBehaviour.Shooter.ProjectileShooter
{
    public interface IShooter
    {
        void SetScalingAndShootByAngle(ProjectileType projectileType, ShootObject shootObject, Shadow shadow, Vector2 position, Vector2 scale, Vector2 moveVector);
    }
}