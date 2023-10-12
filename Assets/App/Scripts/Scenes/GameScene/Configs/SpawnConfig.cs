using System.Collections.Generic;
using App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.FruitFeatures.Enum;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Configs
{
    [CreateAssetMenu(fileName = "new SpawnConfig", menuName = "Configs/Spawn Config")]
    public class SpawnConfig : ScriptableObject
    {
        [MinMaxSlider(1,10)]
        public Vector2Int FruitsAmountRange;
        [MinMaxSlider(1f,10f)]
        public Vector2 GroupSpawnDelayRange;
        [MinMaxSlider(0f,2f)]
        public Vector2 FruitsInGroupSpawnDelayRange;
        [Range(2,100)]
        public int AverageAmountSpawnGroups;
        public List<FruitType> FruitTypes;

    }
}
