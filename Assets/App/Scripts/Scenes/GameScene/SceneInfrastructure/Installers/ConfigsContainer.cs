using App.Scripts.Scenes.GameScene.Configs;
using App.Scripts.Scenes.Infrastructure.CompositeRoot;
using App.Scripts.Scenes.Infrastructure.MonoBehaviourSimulator;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.SceneInfrastructure.Installers
{
    public class ConfigsContainer : MonoBehaviour
    {
        [field: SerializeField] 
        public PhysicsConfig PhysicsConfig { get; private set; }
        [field: SerializeField] 
        public ProjectileConfig ProjectileConfig { get; private set; }
        [field: SerializeField] 
        public ShootConfig ShootConfig { get; private set; }
        [field: SerializeField] 
        public ResourcesConfig ResourcesConfig { get; private set; }
        [field: SerializeField] 
        public ShadowConfig ShadowConfig { get; private set; }
        [field: SerializeField] 
        public HealthConfig HealthConfig { get; private set; }
        [field: SerializeField] 
        public SpawnConfig SpawnConfig { get; private set; }
        [field: SerializeField] 
        public ComboConfig ComboConfig { get; private set; }
        [field: SerializeField] 
        public ScoreConfig ScoreConfig { get; private set; }
        [field: SerializeField] 
        public BonusesConfig BonusesConfig { get; private set; }

    }
}
