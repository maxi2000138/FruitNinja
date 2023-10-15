using App.Scripts.Scenes.GameScene.Configs;
using App.Scripts.Scenes.Infrastructure.CompositeRoot;
using App.Scripts.Scenes.Infrastructure.MonoBehaviourSimulator;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.SceneInfrastructure.Installers
{
    public class ConfigsContainer : Installer
    {
        [field: SerializeField] 
        public GravitationConfig GravitationConfig { get; private set; }
        [field: SerializeField] 
        public FruitConfig FruitConfig { get; private set; }
        [field: SerializeField] 
        public ProjectileConfig ProjectileConfig { get; private set; }
        [field: SerializeField] 
        public ResourcesConfig ResourcesConfig { get; private set; }
        [field: SerializeField] 
        public ShadowConfig ShadowConfig { get; private set; }
        [field: SerializeField] 
        public SpawnConfig SpawnConfig { get; private set; }


        public override void Compose(MonoBehaviourSimulator monoBehaviourSimulator)
        {
           
        }
    }
}
