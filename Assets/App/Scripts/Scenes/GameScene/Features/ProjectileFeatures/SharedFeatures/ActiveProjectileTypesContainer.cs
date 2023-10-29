using System.Collections.Generic;
using App.Scripts.Scenes.GameScene.Configs;

public class ActiveProjectileTypesContainer : IRestartGameListener
{
    public List<ProjectileType> ProjectileTypes { get; private set; }
    public List<ProjectileType> LooseHealthProjectileTypes { get; private set; }
    private readonly SpawnConfig _spawnConfig;

    public ActiveProjectileTypesContainer(SpawnConfig spawnConfig)
    {
        _spawnConfig = spawnConfig;
        ResetActiveProjectiles();
        ResetLooseHealthProjectiles();
    }

    public void OnRestartGame()
    {
        ResetActiveProjectiles();
        ResetLooseHealthProjectiles();
    }

    public void SetActiveProjectiles(List<ProjectileType> projectileTypes)
    {
        ProjectileTypes = projectileTypes;
    }

    public void ResetActiveProjectiles()
    {
        ProjectileTypes = _spawnConfig.ActiveProjectileTypes;
    }
    
    public void SetLooseHealthProjectiles(List<ProjectileType> projectileTypes)
    {
        LooseHealthProjectileTypes = projectileTypes;
    }
    
    public void ResetLooseHealthProjectiles()
    {
        LooseHealthProjectileTypes = _spawnConfig.LooseHealthProjectiles;
    }
}
