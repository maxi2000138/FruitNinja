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
using App.Scripts.Scenes.GameScene.SceneInfrastructure.EntryPoint;
using App.Scripts.Scenes.Infrastructure.CompositeRoot;
using App.Scripts.Scenes.Infrastructure.MonoBehaviourSimulator;
using UnityEngine;
using UnityEngine.Serialization;

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
        private ComboContainer _comboContainer;
        [SerializeField]
        private ConfigsContainer _configsContainer;
        [SerializeField] 
        private ShadowContainer _shadowContainer;
        [SerializeField]
        private Slicer _slicer;
        [SerializeField] 
        private GameEntryPoint _gameEntryPoint;

        [Header("Views")] 
        [SerializeField]
        private ScoreView _currentScoreView;
        [SerializeField]
        private ScoreView _highScoreView;
        [SerializeField]
        private HealthController _healthController;
        [SerializeField]
        private LoosePanelView _loosePanelView;
        [SerializeField]
        private PausePanelView _pausePanelView;
        [SerializeField]
        private ScoreView _scoreView;
        [SerializeField]
        private CustomButton _restartButton;
        [SerializeField]
        private CustomButton _looseExitMenuButton;
        [SerializeField]
        private CustomButton _pauseExitMenuButton;
        [SerializeField]
        private CustomButton _resumeGameButton;
        [SerializeField]
        private CustomButton _pauseGameButton;
        
        private SliceCollidersController _sliceCollidersController;
        private PhysicalFlightCalculator _physicalFlightCalculator;
        private GameStateObserver _gameStateObserver;
        private HealthSystem _healthSystem;
        private HealthOverLoosePolicy _healthOverLoosePolicy;

        public override void OnInstallBindings(MonoBehaviourSimulator monoBehaviourSimulator, ProjectInstaller projectInstaller)
        {
            InputReader = new InputReader();
            ResourceObjectsProvider = new ResourceObjectsProvider();
            ProjectileDestroyer = new ProjectileDestroyer();
            _sliceCollidersController = new SliceCollidersController();
            _gameStateObserver = new GameStateObserver();
            
            _loosePanelView.Construct(projectInstaller.TweenCore);
            _scoreView.Construct(projectInstaller.TweenCore);
            _highScoreView.Construct(projectInstaller.TweenCore);

            
            _physicalFlightCalculator = new PhysicalFlightCalculator(_screenSettingsProvider, _configsContainer.PhysicsConfig);
            _slicer.Construct(InputReader, _screenSettingsProvider, _sliceCollidersController, _configsContainer.ShootConfig);
            DestroyTrigger = new DestroyTrigger(_screenSettingsProvider, ProjectileDestroyer, _configsContainer.ShootConfig);
            _healthSystem = new HealthSystem(_configsContainer.HealthConfig, _healthController, projectInstaller.TweenCore);
            ProjectileFactory = new ProjectileFactory(DestroyTrigger, _projectileContainer, _shadowContainer,
                _sliceCollidersController, ResourceObjectsProvider, _particleSystemPlayer
                , _configsContainer.ProjectileConfig, _configsContainer.ResourcesConfig, _configsContainer.ShadowConfig,
                _healthSystem);
            ShootPolicy = new WavesSpawnPolicy(_configsContainer.SpawnConfig);
            Shooter = new Shooter(ProjectileFactory, _physicalFlightCalculator, _spawnAreasContainer, _screenSettingsProvider,_configsContainer.ShootConfig
                ,_configsContainer.ShadowConfig , _configsContainer.ProjectileConfig, _configsContainer.PhysicsConfig, _configsContainer.SpawnConfig);
            ShootSystem = new ShootSystem(Shooter, ShootPolicy);
            _healthOverLoosePolicy = new HealthOverLoosePolicy(_healthSystem, DestroyTrigger);
            ScoreSystem = new ScoreSystem(projectInstaller.ScoreStateContainer, _slicer, _currentScoreView,
                _highScoreView); 
            _gameEntryPoint.Construct(projectInstaller.SceneLoaderWithCurtains);
            ComboSystem comboSystem = new ComboSystem(_slicer, _configsContainer.ComboConfig, _comboContainer, _screenSettingsProvider);

            PauseController pauseController = new PauseController(_configsContainer.PhysicsConfig);
            RestartGameButton restartGameButton = new RestartGameButton(projectInstaller.SceneLoaderWithCurtains);
            _restartButton.Construct(restartGameButton);
            _looseExitMenuButton.Construct(new ExitToMenuButton(projectInstaller.SceneLoaderWithCurtains));
            _pauseExitMenuButton.Construct(new ExitToMenuButton(projectInstaller.SceneLoaderWithCurtains));
            _pauseGameButton.Construct(new PauseButton(pauseController, _pausePanelView));
            _resumeGameButton.Construct(new ResumeButton(pauseController, _pausePanelView));

            LoosePanelController loosePanelController = new LoosePanelController(_loosePanelView, ScoreSystem);
            
            
            
            monoBehaviourSimulator.AddInitializable(DestroyTrigger);
            monoBehaviourSimulator.AddInitializable(_physicalFlightCalculator);
            monoBehaviourSimulator.AddInitializable(ShootSystem);
            monoBehaviourSimulator.AddUpdatable(DestroyTrigger);
            monoBehaviourSimulator.AddUpdatable(_healthOverLoosePolicy);
            monoBehaviourSimulator.AddUpdatable(InputReader);
            monoBehaviourSimulator.AddDestroyable(ScoreSystem);
            
            _gameStateObserver.AddObserver(_slicer);
            _gameStateObserver.AddObserver(loosePanelController);
            _gameStateObserver.AddObserver(ShootSystem);
            _gameStateObserver.AddObserver(_healthSystem);
            _gameStateObserver.AddObserver(ScoreSystem);
            _gameStateObserver.AddObserver(_healthOverLoosePolicy);
            
            _gameStateObserver.AddPolicy(restartGameButton);
            _gameStateObserver.AddPolicy(_healthOverLoosePolicy);
        }
    }
}
