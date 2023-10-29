using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Configs
{
    [CreateAssetMenu(fileName = "new ShootConfig", menuName = "Configs/Shoot Config")]
    public class ShootConfig : ScriptableObject
    {
        [MinMaxSlider(5f,20f)]
        public Vector2 ShootVelocityRange = new (1f,1f);
        [MinMaxSlider(-1000f,1000f)]
        public Vector2 TorqueVelocityRange = new (1f,1f);
        [Range(-50f,0f)]
        public float DestroyTriggerOffset = 2f;
        [Range(5f, 20f)] 
        public float SliceForce = 1f;
        [Range(0.5f, 10f)]
        public float MinSliceSpeed = 0.1f;
    }
}
