using App.Scripts.Scenes.Infrastructure.CompositeRoot;
using App.Scripts.Scenes.Infrastructure.MonoBehaviourSimulator;
using UnityEngine;
using UnityEngine.Serialization;

namespace App.Scripts.Scenes.GameScene.SceneInfrastructure.EntryPoint
{
    public class GameEntryPoint : EntryPointBehaviour
    {
        [FormerlySerializedAs("_compositionOrder")] [SerializeField] 
        private InstallersOrder _installersOrder;
        [SerializeField] 
        private MonoBehaviourSimulator _monoBehaviourSimulator = new();
        

        public override void OnEntryPoint()
        {
            _installersOrder.CompositeAll(_monoBehaviourSimulator);
            _monoBehaviourSimulator.InitializeAll();
        }
    }
}
