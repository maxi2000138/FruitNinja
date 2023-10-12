using System.Collections.Generic;
using App.Scripts.Scenes.GameScene.Configs;
using App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.FruitFeatures.Enum;
using Sirenix.OdinInspector;

namespace App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.FruitFeatures.Data
{
    public class FruitDataDictionary : SerializedComponent
    {
        public Dictionary<FruitType, List<FruitData>> FruitDictionary;
    }
}
