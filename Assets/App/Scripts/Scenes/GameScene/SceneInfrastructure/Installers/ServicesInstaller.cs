using UnityEngine;

public class ServicesInstaller : CompositeRoot
{
    [Header("SceneObjects")]
    [SerializeField] 
    private Camera _camera;
    [Header("Configs")]
    [SerializeField] 
    private GameConfig _gameConfig;
    [SerializeField] 
    private FruitConfig _fruitConfig;
    [SerializeField] 
    private ResourcesConfig _resourcesConfig;
    [SerializeField] 
    private ProjectileConfig _projectileConfig;
    [Header("MonoBehaviourScripts")] 
    [SerializeField]
    private EntryPoint _entryPoint;
    [SerializeField]
    private SpawnAreasContainer _spawnAreasContainer;
    [SerializeField] 
    private ProjectileContainer _projectileContainer;
    [SerializeField]
    private CoroutineRunner _coroutineRunner;

    private ResourceObjectsProvider _resourceObjectsProvider;
    private ScreenSettingsProvider _screenSettingsProvider;
    private ProjectileDestroyer _projectileDestroyer;
    private ProjectileFactory _projectileFactory;
    private ProjectileShooter _projectileShooter;
    private IShootPolicy _shootPolicy;
    private DestroyTrigger _destroyTrigger;

    public override void Compose(MonoBehaviourSimulator monoBehaviourSimulator)
    {
        _screenSettingsProvider = new ScreenSettingsProvider(_camera);
        _resourceObjectsProvider = new ResourceObjectsProvider();
        _projectileDestroyer = new ProjectileDestroyer();
        _destroyTrigger = new DestroyTrigger(_screenSettingsProvider, _projectileDestroyer, _gameConfig);
        _projectileFactory = new ProjectileFactory(_destroyTrigger, _projectileContainer, _resourceObjectsProvider, _fruitConfig, _resourcesConfig);
        _shootPolicy = new BichShootPolicy(_coroutineRunner);
        _projectileShooter = new ProjectileShooter(_projectileFactory, _spawnAreasContainer, _screenSettingsProvider,_projectileConfig, _gameConfig,_shootPolicy);
        _entryPoint.Construct(_projectileShooter);
        
        monoBehaviourSimulator.AddInitializable(_projectileShooter);
        monoBehaviourSimulator.AddInitializable(_destroyTrigger);
        monoBehaviourSimulator.AddUpdatable(_destroyTrigger);
    }
}
