using System.Collections.Generic;
using App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.FruitFeatures.Enum;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace App.Scripts.Scenes.GameScene.Configs
{
    [CreateAssetMenu(fileName = "new SpawnConfig", menuName = "Configs/Spawn Config")]
    public class SpawnConfig : SerializedScriptableObject
    {
        [FormerlySerializedAs("FruitsAmountRange")] [MinMaxSlider(1,25)]
        public Vector2Int BlocksAmountRange;
        [MinMaxSlider(1f,10f)]
        public Vector2 GroupSpawnDelayRange;
        [FormerlySerializedAs("FruitsInGroupSpawnDelayRange")] [MinMaxSlider(0f,2f)]
        public Vector2 BlocksInGroupSpawnDelayRange;
        [Range(2,100)]
        public int AverageAmountSpawnGroups;
        [FormerlySerializedAs("FruitTypes")] public List<FruitType> ActiveFruitTypes;
        [FormerlySerializedAs("ActiveBonusesTypes")] public List<ProjectileType> ActiveProjectileTypes;
        public List<ProjectileType> LooseHealthProjectiles;
        public Dictionary<ProjectileType, float> ProjectileSpawnProbability;
    }
}
