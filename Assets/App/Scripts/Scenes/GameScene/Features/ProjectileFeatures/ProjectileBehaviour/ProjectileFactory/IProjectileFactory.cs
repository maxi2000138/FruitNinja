using App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.FruitFeatures.Enum;
using App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.FruitFeatures.Fruit;
using App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.ShadowFeatures;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.ProjectileBehaviour.ProjectileFactory
{
    public interface IProjectileFactory
    {
        Fruit CreateFruit(FruitType fruitType, Vector2 position, Vector2 fruitScale, Vector2 shadowScale, out Shadow shadow);
        Bomb CreateBomb(Vector2 position, Vector2 fruitScale, Vector2 shadowScale, out Shadow shadow);
        Heart CreateHeart(Vector2 position, Vector2 fruitScale, Vector2 shadowScale, out Shadow shadow);
        Magnet CreateMagnet(Vector2 position, Vector2 magnetSclae, Vector2 shadowScale, out Shadow shadow);
        Brick CreateBrick(Vector2 position, Vector2 magnetScale, Vector2 shadowScale, out Shadow shadow);
        Ice CreateIce(Vector2 position, Vector2 iceScale, Vector2 shadowScale, out Shadow shadow);
    }
}