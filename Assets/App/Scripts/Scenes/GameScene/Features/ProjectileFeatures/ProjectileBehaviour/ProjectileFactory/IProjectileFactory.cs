using App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.FruitFeatures.Enum;
using App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.FruitFeatures.Fruit;
using App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.ShadowFeatures;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.ProjectileBehaviour.ProjectileFactory
{
    public interface IProjectileFactory
    {
        Fruit CreateFruitWithShadow(FruitType fruitType, Vector2 position, Vector2 scale, out Shadow shadow);
    }
}