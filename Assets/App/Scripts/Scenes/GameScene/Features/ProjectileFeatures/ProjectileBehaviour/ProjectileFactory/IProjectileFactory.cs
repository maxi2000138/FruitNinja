using UnityEngine;

public interface IProjectileFactory
{
    WholeFruit CreateFruitByType(Vector2 position, FruitType fruitType);
}