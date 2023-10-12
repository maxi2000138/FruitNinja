using App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.ProjectileBehaviour.ShootSystem;
using App.Scripts.Scenes.Infrastructure.CompositeRoot;
using App.Scripts.Scenes.Infrastructure.MonoBehaviourSimulator;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.SceneInfrastructure.EntryPoint
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] 
        private CompositionOrder _compositionOrder;

        private readonly MonoBehaviourSimulator _monoBehaviourSimulator = new();
        private ShootSystem _shootSystem;

        public void Construct(ShootSystem shootSystem)
        {
            _shootSystem = shootSystem;
        }

        public void Awake()
        {
            SetGameSettings();
        
            _compositionOrder.CompositeAll(_monoBehaviourSimulator);
            _monoBehaviourSimulator.InitializeAll();
        
            _shootSystem.StartShooting();
        }

        private void Update()
        {
            _monoBehaviourSimulator.UpdateAll(Time.deltaTime);
        }

        private static void SetGameSettings()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
        }
    }
}
