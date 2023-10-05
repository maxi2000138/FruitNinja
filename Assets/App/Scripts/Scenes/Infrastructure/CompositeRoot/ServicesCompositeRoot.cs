using UnityEngine;

public class ServicesCompositeRoot : CompositeRoot
{
    [Header("SceneObjects")]
    [SerializeField] 
    private Camera _camera;
    [Header("Configs")]
    [SerializeField] 
    private GameConfig _gameConfig;
    [SerializeField] 
    private FruitConfig _fruitConfig;
    [Header("MonoBehaviourScripts")] 
    [SerializeField]
    private EntryPoint _entryPoint;
    [SerializeField]
    private SpawnAreasContainer _spawnAreasContainer;
    [SerializeField] 
    private ProjectileContainer _projectileContainer;
    [SerializeField]
    private CoroutineRunner _coroutineRunner;

    private ScreenSettingsProvider _screenSettingsProvider;
    private ProjectileDestroyer _projectileDestroyer;
    private DestroyLine _destroyLine;
    private ProjectileFactory _projectileFactory;
    private ProjectileShooter _projectileShooter;
    private IShootPolicy _shootPolicy;

    public override void Compose(MonoBehaviourSimulator monoBehaviourSimulator)
    {
        _screenSettingsProvider = new ScreenSettingsProvider(_camera);
        _projectileDestroyer = new ProjectileDestroyer();
        _destroyLine = new DestroyLine(_screenSettingsProvider, _projectileDestroyer, _gameConfig);
        _projectileFactory = new ProjectileFactory(_destroyLine, _projectileContainer, _fruitConfig);
        _shootPolicy = new BichShootPolicy(_coroutineRunner);
        _projectileShooter = new ProjectileShooter(_projectileFactory, _spawnAreasContainer, _screenSettingsProvider, _gameConfig,_shootPolicy);
        _entryPoint.Construct(_projectileShooter);
        
        monoBehaviourSimulator.AddInitializable(_projectileShooter);
        monoBehaviourSimulator.AddInitializable(_destroyLine);
        monoBehaviourSimulator.AddUpdatable(_destroyLine);
    }
}
