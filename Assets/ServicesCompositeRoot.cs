using UnityEngine;

public class ServicesCompositeRoot : CompositeRoot
{
    [SerializeField] 
    private Camera _camera;
    [SerializeField] 
    private SpawnAreasContainer _spawnAreasContainer;
    [SerializeField] 
    private GameConfig _gameConfig;
    [SerializeField] 
    private ProjectileContainer _projectileContainer;

    public override void Compose(MonoBehaviourSimulator monoBehaviourSimulator)
    {
        CameraFeaturesProvider cameraFeaturesProvider = new CameraFeaturesProvider(_camera);
        ProjectileDestroyer projectileDestroyer = new ProjectileDestroyer();
        DestroyLine destroyLine = new DestroyLine(cameraFeaturesProvider, projectileDestroyer, _gameConfig);
        ProjectileFactory projectileFactory = new ProjectileFactory(destroyLine, _projectileContainer);
        ProjectileShooter projectileShooter = new ProjectileShooter(projectileFactory, _spawnAreasContainer, cameraFeaturesProvider);
        
        monoBehaviourSimulator.AddInitializable(destroyLine);
        monoBehaviourSimulator.AddUpdatable(projectileShooter);
        monoBehaviourSimulator.AddUpdatable(destroyLine);
    }
}
