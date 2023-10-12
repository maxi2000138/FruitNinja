using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.ProjectileBehaviour.ProjectileDestroyer.ProjectileDestroyer
{
    public class ProjectileDestroyer : IProjectileDestroyer
    {
        public void DestroyProjectile(GameObject projectile)
        {
            Object.Destroy(projectile);   
        }
    }
}