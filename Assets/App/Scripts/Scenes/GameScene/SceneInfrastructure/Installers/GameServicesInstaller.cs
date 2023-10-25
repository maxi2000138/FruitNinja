using App.Scripts.Scenes.GameScene.Features.CameraFeatures.ScreenSettingsProvider;
using App.Scripts.Scenes.GameScene.Features.InputFeatures;
using App.Scripts.Scenes.GameScene.Features.ParticleFeatures;
using App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ColliderFeatures;
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
        [FormerlySerializedAs("_nameprojectilesParenter")] [FormerlySerializedAs("_projectilesSpawnParent")] [SerializeField] 
        private ProjectilesParenter _projectilesParenter;
        [FormerlySerializedAs("_namecomboParenter")] [FormerlySerializedAs("_comboContainer")] [SerializeField] 
        private ComboParenter _comboParenter;
        [SerializeField]
        private ConfigsContainer _configsContainer;
        [FormerlySerializedAs("_shadowContainer")] [SerializeField] 
        private ShadowParenter _shadowParenter;
        [SerializeField]
        private Slicer _slicer;
        [SerializeField] 
        private GameEntryPoint _gameEntryPoint;
        [SerializeField] 
        private MagnetSuction _magnetSuction;

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
        private ProjectileContainer _projectileContainer;

        public override void OnInstallBindings(MonoBehaviourSimulator monoBehaviourSimulator, ProjectInstaller projectInstaller)
        {
            InputReader = new InputReader();
            ResourceObjectsProvider = new ResourceObjectsProvider();
            ProjectileDestroyer = new ProjectileDestroyer();
            _sliceCollidersController = new SliceCollidersController();
            _gameStateObserver = new GameStateObserver();
            _projectileContainer = new ProjectileContainer();

            _loosePanelView.Construct(projectInstaller.TweenCore);
            _scoreView.Construct(projectInstaller.TweenCore);
            _highScoreView.Construct(projectInstaller.TweenCore);

            
            _physicalFlightCalculator = new PhysicalFlightCalculator(_screenSettingsProvider, _configsContainer.PhysicsConfig);
            _slicer.Construct(InputReader, _screenSettingsProvider, _sliceCollidersController, _configsContainer.ShootConfig);
            DestroyTrigger = new DestroyTrigger(_screenSettingsProvider, _projectileContainer, ProjectileDestroyer, _configsContainer.ShootConfig);
            _healthSystem = new HealthSystem(_configsContainer.HealthConfig, _healthController, projectInstaller.TweenCore);
            ProjectileFactory = new ProjectileFactory(DestroyTrigger, _projectilesParenter, _shadowParenter,
                _sliceCollidersController, ResourceObjectsProvider, _particleSystemPlayer
                , _configsContainer.ProjectileConfig, _configsContainer.ResourcesConfig, _configsContainer.ShadowConfig,
                _healthSystem, _configsContainer.BonusesConfig, _projectileContainer, _slicer);
            ShootPolicy = new WavesSpawnPolicy(_configsContainer.SpawnConfig);
            Shooter = new Shooter(ProjectileFactory, _physicalFlightCalculator, _spawnAreasContainer, _screenSettingsProvider,_configsContainer.ShootConfig
                ,_configsContainer.ShadowConfig , _configsContainer.ProjectileConfig, _configsContainer.PhysicsConfig, _configsContainer.SpawnConfig);
            ShootSystem = new ShootSystem(Shooter, ShootPolicy);
            _healthOverLoosePolicy = new HealthOverLoosePolicy(_healthSystem, DestroyTrigger);
            ScoreSystem = new ScoreSystem(projectInstaller.ScoreStateContainer, _slicer, _currentScoreView,
                _highScoreView, _configsContainer.ScoreConfig); 
            _gameEntryPoint.Construct(projectInstaller.SceneLoaderWithCurtains);
            ComboSystem comboSystem = new ComboSystem(_slicer, _configsContainer.ComboConfig, _comboParenter, _screenSettingsProvider, ScoreSystem, _configsContainer.ScoreConfig);
            _magnetSuction.Construct(_projectileContainer, _configsContainer.BonusesConfig);

            PauseController pauseController = new PauseController(_configsContainer.PhysicsConfig);
            RestartGameButton restartGameButton = new RestartGameButton(projectInstaller.SceneLoaderWithCurtains);
            _restartButton.Construct(restartGameButton, projectInstaller.TweenCore);
            _looseExitMenuButton.Construct(new ExitToMenuButton(projectInstaller.SceneLoaderWithCurtains), projectInstaller.TweenCore);
            _pauseExitMenuButton.Construct(new ExitToMenuButton(projectInstaller.SceneLoaderWithCurtains), projectInstaller.TweenCore);
            _pauseGameButton.Construct(new PauseButton(pauseController, _pausePanelView), projectInstaller.TweenCore);
            _resumeGameButton.Construct(new ResumeButton(pauseController, _pausePanelView), projectInstaller.TweenCore);

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
