using App.Scripts.Scenes.GameScene.Features.CameraFeatures.ScreenSettingsProvider;
using App.Scripts.Scenes.GameScene.Features.InputFeatures;
using App.Scripts.Scenes.GameScene.Features.ParticleFeatures;
using App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ColliderFeatures;
using App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.ProjectileBehaviour.ProjectileContainer;
using App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.ProjectileBehaviour.ProjectileDestroyer.DestroyTrigger;
using App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.ProjectileBehaviour.ProjectileDestroyer.ProjectileDestroyer;
using App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.ProjectileBehaviour.ProjectileFactory;
using App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.ProjectileBehaviour.Shooter.ProjectileShooter;
using App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.ProjectileBehaviour.Shooter.ShootPolicy;
using App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.ProjectileBehaviour.ShootSystem;
using App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.ShadowFeatures;
using App.Scripts.Scenes.GameScene.Features.ResourceFeatures;
using App.Scripts.Scenes.GameScene.Features.SpawnAreaFeatures;
using App.Scripts.Scenes.Infrastructure.CompositeRoot;
using App.Scripts.Scenes.Infrastructure.CoroutineRunner;
using App.Scripts.Scenes.Infrastructure.MonoBehaviourSimulator;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.SceneInfrastructure.Installers
{
    public class GameServicesInstaller : InstallerBehaviour
    {
        public ResourceObjectsProvider ResourceObjectsProvider { get; private set; }
        public ProjectileDestroyer ProjectileDestroyer { get; private set; }
        public ProjectileFactory ProjectileFactory { get; private set; }
        public DestroyTrigger DestroyTrigger { get; private set; }
        public IShootPolicy ShootPolicy { get; private set; }
        public ShootSystem ShootSystem { get; private set; }
        public InputReader InputReader { get; private set; }
        public Shooter Shooter { get; private set; }
        public ScoreSystem ScoreSystem { get; private set; }

        [SerializeField]
        private ScreenSettingsProvider _screenSettingsProvider;
        [SerializeField] 
        private ParticleSystemPlayer _particleSystemPlayer;
        [SerializeField]
        private SpawnAreasContainer _spawnAreasContainer;
        [SerializeField] 
        private ProjectileContainer _projectileContainer;
        [SerializeField]
        private ConfigsContainer _configsContainer;
        [SerializeField] 
        private ShadowContainer _shadowContainer;
        [SerializeField]
        private CoroutineRunner _coroutineRunner;
        [SerializeField]
        private Slicer _slicer;

        [Header("Views")] 
        [SerializeField]
        private ScoreView _currentScoreView;
        [SerializeField]
        private ScoreView _highScoreView;
    
        private SliceCollidersController _sliceCollidersController;
        private PhysicalFlightCalculator _physicalFlightCalculator;


        public override void InstallBindings(MonoBehaviourSimulator monoBehaviourSimulator)
        {
            ProjectInstaller projectInstaller = FindObjectOfType<ProjectInstaller>();

            InputReader = new InputReader();
            ResourceObjectsProvider = new ResourceObjectsProvider();
            ProjectileDestroyer = new ProjectileDestroyer();
            _sliceCollidersController = new SliceCollidersController();
            _physicalFlightCalculator = new PhysicalFlightCalculator(_screenSettingsProvider, _configsContainer.GravitationConfig);
            _slicer.Construct(InputReader, _screenSettingsProvider, _sliceCollidersController, _configsContainer.ProjectileConfig);
            DestroyTrigger = new DestroyTrigger(_screenSettingsProvider, ProjectileDestroyer, _configsContainer.ProjectileConfig);
            ProjectileFactory = new ProjectileFactory(DestroyTrigger, _projectileContainer, _shadowContainer, _sliceCollidersController, ResourceObjectsProvider, _particleSystemPlayer
                , _configsContainer.FruitConfig, _configsContainer.ResourcesConfig, _configsContainer.ShadowConfig);
            ShootPolicy = new WavesSpawnPolicy(_coroutineRunner, _configsContainer.SpawnConfig);
            Shooter = new Shooter(ProjectileFactory, _physicalFlightCalculator, _spawnAreasContainer, _screenSettingsProvider,_configsContainer.ProjectileConfig
                ,_configsContainer.ShadowConfig , _configsContainer.FruitConfig, _configsContainer.GravitationConfig, _configsContainer.SpawnConfig);
            ShootSystem = new ShootSystem(Shooter, ShootPolicy);
            ScoreSystem = new ScoreSystem(projectInstaller.ScoreStateContainer, _slicer, _currentScoreView,
                _highScoreView);
 
            monoBehaviourSimulator.AddInitializable(DestroyTrigger);
            monoBehaviourSimulator.AddInitializable(_physicalFlightCalculator);
            monoBehaviourSimulator.AddInitializable(ShootSystem);
            monoBehaviourSimulator.AddUpdatable(DestroyTrigger);
            monoBehaviourSimulator.AddUpdatable(InputReader);
            monoBehaviourSimulator.AddDestroyable(ScoreSystem);
        }
    }
}
