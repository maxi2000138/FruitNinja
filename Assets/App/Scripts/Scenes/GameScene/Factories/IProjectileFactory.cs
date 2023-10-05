using UnityEngine;

public interface IProjectileFactory
{
    GameObject CreateFruitByType(Vector2 position, FruitType fruitType);
}