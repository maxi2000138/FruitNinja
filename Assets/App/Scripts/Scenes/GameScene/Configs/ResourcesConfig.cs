using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Configs
{
    [CreateAssetMenu(fileName = "new ResourcesConfig", menuName = "Configs/Resources Config")]
    public class ResourcesConfig : SerializedScriptableObject
    {
        public Dictionary<ProjectileType, Dictionary<ProjectilePartEnum, string>> PartPathes;
        public string ShadowPath = "Prefabs/Shadow";
    }
}
