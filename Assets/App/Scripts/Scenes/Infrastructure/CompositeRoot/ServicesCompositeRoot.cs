using UnityEngine;

public class ServicesCompositeRoot : CompositeRoot
{
    [Header("SceneObjects")]
    [SerializeField] 
    private Camera _camera;
    [SerializeField] 
    private GameConfig _gameConfig;

    [Header("MonoBehaviourServices")] 
    [SerializeField]
    private EntryPoint _entryPoint;
    [SerializeField]
    private SpawnAreasContainer _spawnAreasContainer;
    [SerializeField] 
    private ProjectileContainer _projectileContainer;
    [SerializeField]
    private CoroutineRunner _coroutineRunner;

    public override void Compose(MonoBehaviourSimulator monoBehaviourSimulator)
    {
        CameraFeaturesProvider cameraFeaturesProvider = new CameraFeaturesProvider(_camera);
        ProjectileDestroyer projectileDestroyer = new ProjectileDestroyer();
        DestroyLine destroyLine = new DestroyLine(cameraFeaturesProvider, projectileDestroyer, _gameConfig);
        ProjectileFactory projectileFactory = new ProjectileFactory(destroyLine, _projectileContainer);
        ProjectileShooter projectileShooter = new ProjectileShooter(projectileFactory, _spawnAreasContainer, cameraFeaturesProvider,_coroutineRunner, _gameConfig);
        _entryPoint.Construct(projectileShooter);
        
        monoBehaviourSimulator.AddInitializable(destroyLine);
        monoBehaviourSimulator.AddUpdatable(destroyLine);
    }
}
