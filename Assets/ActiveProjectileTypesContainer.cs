using System.Collections.Generic;
using App.Scripts.Scenes.GameScene.Configs;

public class ActiveProjectileTypesContainer
{
    public List<ProjectileType> ProjectileTypes { get; private set; }
    private readonly SpawnConfig _spawnConfig;

    public ActiveProjectileTypesContainer(SpawnConfig spawnConfig)
    {
        _spawnConfig = spawnConfig;
    }
    
    public void SetActiveProjectiles(List<ProjectileType> projectileTypes)
    {
        ProjectileTypes = projectileTypes;
    }

    public void RestActiveProjectile()
    {
        ProjectileTypes = _spawnConfig.ActiveProjectileTypes;
    }
}
