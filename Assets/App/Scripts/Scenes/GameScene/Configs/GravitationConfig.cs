using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Configs
{
    [CreateAssetMenu(fileName = "new GameConfig", menuName = "Configs/Game Config")]
    public class GravitationConfig : ScriptableObject
    {
        public float StartGravityValue = -5f;
    }
}
