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

    private CameraFeaturesProvider _cameraFeaturesProvider;
    private ProjectileDestroyer _projectileDestroyer;
    private DestroyLine _destroyLine;
    private ProjectileFactory _projectileFactory;
    private ProjectileShooter _projectileShooter;
    private IShootPolicy _shootPolicy;

    public override void Compose(MonoBehaviourSimulator monoBehaviourSimulator)
    {
        _cameraFeaturesProvider = new CameraFeaturesProvider(_camera);
        _projectileDestroyer = new ProjectileDestroyer();
        _destroyLine = new DestroyLine(_cameraFeaturesProvider, _projectileDestroyer, _gameConfig);
        _projectileFactory = new ProjectileFactory(_destroyLine, _projectileContainer);
        _shootPolicy = new BichShootPolicy(_coroutineRunner);
        _projectileShooter = new ProjectileShooter(_projectileFactory, _spawnAreasContainer, _cameraFeaturesProvider, _gameConfig,_shootPolicy);
        _entryPoint.Construct(_projectileShooter);
        
        monoBehaviourSimulator.AddInitializable(_projectileShooter);
        monoBehaviourSimulator.AddInitializable(_destroyLine);
        monoBehaviourSimulator.AddUpdatable(_destroyLine);
    }
}
