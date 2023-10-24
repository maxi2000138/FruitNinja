using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.ProjectileBehaviour.ProjectileDestroyer.ProjectileDestroyer
{
    public interface IProjectileDestroyer
    {
        void DestroyProjectiles(params GameObject[] projectile);
    }
}