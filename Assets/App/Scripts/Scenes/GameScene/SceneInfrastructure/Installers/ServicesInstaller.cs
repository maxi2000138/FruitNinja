using UnityEngine;

public class ServicesInstaller : Installer
{
    public InputReader InputReader { get; private set; }
    public ResourceObjectsProvider ResourceObjectsProvider { get; private set; }
    public ProjectileDestroyer ProjectileDestroyer { get; private set; }
    public ProjectileFactory ProjectileFactory { get; private set; }
    public Shooter Shooter { get; private set; }
    public IShootPolicy ShootPolicy { get; private set; }
    public DestroyTrigger DestroyTrigger { get; private set; }
    public ShootSystem ShootSystem { get; private set; }
    
    [Header("Installers")] 
    [SerializeField]
    private ConfigsInstaller _configsInstaller;
    [Header("MonoBehaviourScripts")] 
    [SerializeField]
    private ScreenSettingsProvider _screenSettingsProvider;
    [SerializeField]
    private SpawnAreasContainer _spawnAreasContainer;
    [SerializeField] 
    private ProjectileContainer _projectileContainer;
    [SerializeField]
    private SliceDrawer _sliceDrawer;
    [SerializeField] 
    private ShadowContainer _shadowContainer;
    [SerializeField]
    private CoroutineRunner _coroutineRunner;
    
    

    public override void Compose(MonoBehaviourSimulator monoBehaviourSimulator)
    {
        InputReader = new InputReader();
        ResourceObjectsProvider = new ResourceObjectsProvider();
        ProjectileDestroyer = new ProjectileDestroyer();
        _sliceDrawer.Construct(InputReader, _screenSettingsProvider, _coroutineRunner);
        DestroyTrigger = new DestroyTrigger(_screenSettingsProvider, ProjectileDestroyer, _configsInstaller.ProjectileConfig);
        ProjectileFactory = new ProjectileFactory(DestroyTrigger, _projectileContainer, _shadowContainer, ResourceObjectsProvider
            , _configsInstaller.FruitConfig, _configsInstaller.ResourcesConfig, _configsInstaller.ShadowConfig);
        ShootPolicy = new WavesSpawnPolicy(_coroutineRunner, _configsInstaller.SpawnConfig);
        Shooter = new Shooter(ProjectileFactory, _spawnAreasContainer, _screenSettingsProvider,_configsInstaller.ProjectileConfig
            ,_configsInstaller.ShadowConfig , _configsInstaller.FruitConfig, _configsInstaller.GravitationConfig);
        ShootSystem = new ShootSystem(Shooter, ShootPolicy);

        monoBehaviourSimulator.AddInitializable(Shooter);
        monoBehaviourSimulator.AddInitializable(DestroyTrigger);
        monoBehaviourSimulator.AddUpdatable(DestroyTrigger);
    }
}
