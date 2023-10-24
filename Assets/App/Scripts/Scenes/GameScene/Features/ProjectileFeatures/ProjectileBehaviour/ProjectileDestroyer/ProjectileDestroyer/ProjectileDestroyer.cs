using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.ProjectileBehaviour.ProjectileDestroyer.ProjectileDestroyer
{
    public class ProjectileDestroyer : IProjectileDestroyer
    {
        public void DestroyProjectiles(params GameObject[] projectiles)
        {
            for(int i = 0; i < projectiles.Length; i++)
                Object.Destroy(projectiles[i]);   
        }
    }
}