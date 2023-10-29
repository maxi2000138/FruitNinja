using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Configs
{
    [CreateAssetMenu(fileName = "new PhysicsConfig", menuName = "Configs/Physics Config")]
    public class PhysicsConfig : ScriptableObject
    {
        public float StartGravityValue = -5f;
        public float HighestPointOffset = 1f;
    }
}
