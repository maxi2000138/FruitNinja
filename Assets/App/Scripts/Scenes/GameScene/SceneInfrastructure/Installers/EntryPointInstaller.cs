using App.Scripts.Scenes.Infrastructure.CompositeRoot;
using App.Scripts.Scenes.Infrastructure.MonoBehaviourSimulator;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.SceneInfrastructure.Installers
{
    public class EntryPointInstaller : Installer
    {
        [SerializeField] 
        private EntryPoint.EntryPoint _entryPoint;
        [Header("Installers")]
        [SerializeField] 
        private ServicesInstaller _servicesInstaller;
    
        public override void Compose(MonoBehaviourSimulator monoBehaviourSimulator)
        {
            _entryPoint.Construct(_servicesInstaller.ShootSystem);
        }
    }
}
