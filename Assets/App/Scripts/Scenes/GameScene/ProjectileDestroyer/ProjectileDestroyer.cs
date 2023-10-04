using UnityEngine;

public class ProjectileDestroyer : IProjectileDestroyer
{
    public void DestroyProjectile(GameObject projectile)
    {
        Object.Destroy(projectile);   
    }
}