using UnityEngine;

public class ProjectileFactory : IProjectileFactory
{
    private readonly IDestroyLine _destroyLine;
    private readonly ProjectileContainer _projectileContainer;
    private readonly FruitConfig _fruitConfig;

    public ProjectileFactory(IDestroyLine destroyLine, ProjectileContainer projectileContainer, FruitConfig fruitConfig)
    {
        _destroyLine = destroyLine;
        _projectileContainer = projectileContainer;
        _fruitConfig = fruitConfig;
    }

    public GameObject CreateFruitByType(Vector2 position, FruitType fruitType)
    {
        GameObject fruitObject = GameObject.Instantiate((GameObject)Resources.Load(ResourcePathes.FruitPath), position ,Quaternion.identity, _projectileContainer.transform);
        Fruit fruit = fruitObject.GetComponent<Fruit>();
        FruitData fruitData;
        if (_fruitConfig.FruitDictionary.TryGetValue(fruitType, out fruitData))
        {
            fruit.SetFruitSprite(fruitData.Sprite, fruitData.SpriteScale);
        }
        _destroyLine.AddLineDestroyListener(fruitObject.transform);
        return fruitObject;
    }
}
