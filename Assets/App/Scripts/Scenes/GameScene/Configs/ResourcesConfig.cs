using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Configs
{
    [CreateAssetMenu(fileName = "new ResourcesConfig", menuName = "Configs/Resources Config")]
    public class ResourcesConfig : ScriptableObject
    {
        public string FruitPath = "Prefabs/Fruit";
        public string FruitPartPath = "Prefabs/FruitPart";
        public string ShadowPath = "Prefabs/Shadow";
    }
}
