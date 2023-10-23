using System;
using System.Collections.Generic;
using App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.FruitFeatures.Enum;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Configs
{
    [CreateAssetMenu(fileName = "new ProjectileConfig", menuName = "Configs/Projectile Config")]
    public class ProjectileConfig : SerializedScriptableObject
    {
        public Dictionary<FruitType, FruitData> FruitDictionary;
        public Dictionary<BonusesType, BonusData> BonusesDictionary;
        public Dictionary<ProjectileType, ScaleData> ProjectileScales;
    }

    [Serializable]
    public class FruitData
    {
        public Sprite FullSprite;
        public Dictionary<ProjectilePartEnum, Sprite> PartSprites;
        public Color SliceColor;
    }
    
    [Serializable]
    public class BonusData
    {
        public Dictionary<ProjectilePartEnum, Sprite> PartSprites;
    }

    [Serializable]
    public class ScaleData
    {
        [MinMaxSlider(0.1f, 2f)]
        public Vector2 Scale;
    }
}