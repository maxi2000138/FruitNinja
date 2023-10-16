using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Configs
{
    [CreateAssetMenu(fileName = "new ResourcesConfig", menuName = "Configs/Resources Config")]
    public class ResourcesConfig : SerializedScriptableObject
    {
        public Dictionary<ProjectilePartEnum, string> FruitsPartPath;
        public string ShadowPath = "Prefabs/Shadow";
    }
}
