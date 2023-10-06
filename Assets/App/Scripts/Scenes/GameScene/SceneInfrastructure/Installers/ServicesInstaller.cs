using System.Collections.Generic;
using UnityEngine;

public class ServicesInstaller : Installer
{
    [Header("Installers")] 
    [SerializeField]
    private ConfigsInstaller _configsInstaller;
    [Header("MonoBehaviourScripts")] 
    [SerializeField]
    private EntryPoint _entryPoint;
    [SerializeField]
    private ScreenSettingsProvider _screenSettingsProvider;
    [SerializeField]
    private SpawnAreasContainer _spawnAreasContainer;
    [SerializeField] 
    private ProjectileContainer _projectileContainer;
    [SerializeField]
    private CoroutineRunner _coroutineRunner;

    public ResourceObjectsProvider ResourceObjectsProvider { get; private set; }
    public ProjectileDestroyer ProjectileDestroyer { get; private set; }
    public ProjectileFactory ProjectileFactory { get; private set; }
    public ProjectileShooter ProjectileShooter { get; private set; }
    public IShootPolicy ShootPolicy { get; private set; }
    public DestroyTrigger DestroyTrigger { get; private set; }

    public override void Compose(MonoBehaviourSimulator monoBehaviourSimulator)
    {
        ResourceObjectsProvider = new ResourceObjectsProvider();
        ProjectileDestroyer = new ProjectileDestroyer();
        DestroyTrigger = new DestroyTrigger(_screenSettingsProvider, ProjectileDestroyer, _configsInstaller.GameConfig);
        ProjectileFactory = new ProjectileFactory(DestroyTrigger, _projectileContainer, ResourceObjectsProvider, _configsInstaller.FruitConfig, _configsInstaller.ResourcesConfig);
        ShootPolicy = new BichShootPolicy(_coroutineRunner);
        ProjectileShooter = new ProjectileShooter(ProjectileFactory, _spawnAreasContainer, _screenSettingsProvider,_configsInstaller.ProjectileConfig, _configsInstaller.GameConfig,ShootPolicy);
        _entryPoint.Construct(ProjectileShooter);
        List<ProjectileConfig> listik = new();

        monoBehaviourSimulator.AddInitializable(ProjectileShooter);
        monoBehaviourSimulator.AddInitializable(DestroyTrigger);
        monoBehaviourSimulator.AddUpdatable(DestroyTrigger);
    }
}
