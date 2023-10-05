using UnityEngine;

public interface IProjectileFactory
{
    Fruit CreateFruitByType(Vector2 position, FruitType fruitType);
}