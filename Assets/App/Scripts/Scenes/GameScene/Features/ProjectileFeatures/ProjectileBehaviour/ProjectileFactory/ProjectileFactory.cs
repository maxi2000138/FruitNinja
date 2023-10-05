using UnityEngine;

public class ProjectileFactory : IProjectileFactory
{
    private readonly ResourceObjectsProvider _resourceObjectsProvider;
    private readonly ResourcesConfig _resourcesConfig;
    private readonly ProjectileContainer _projectileContainer;
    private readonly IDestroyTrigger _destroyTrigger;
    private readonly FruitConfig _fruitConfig;

    public ProjectileFactory(IDestroyTrigger destroyTrigger, ProjectileContainer projectileContainer, ResourceObjectsProvider resourceObjectsProvider,
        FruitConfig fruitConfig, ResourcesConfig resourcesConfig)
    {
        _destroyTrigger = destroyTrigger;
        _projectileContainer = projectileContainer;
        _fruitConfig = fruitConfig;
        _resourceObjectsProvider = resourceObjectsProvider;
        _resourcesConfig = resourcesConfig;
    }

    public Fruit CreateFruitByType(Vector2 position, FruitType fruitType)
    {
        Fruit fruit = SpawnFruit(position, _projectileContainer.transform).GetComponent<Fruit>();
        if (_fruitConfig.FruitDictionary.TryGetValue(fruitType, out var fruitData))
        {
            fruit.SetFruitSprite(fruitData.Sprite, fruitData.SpriteScale);
        }
        _destroyTrigger.AddLineDestroyListener(fruit.transform);
        return fruit;
    }

    private GameObject SpawnFruit(Vector2 position, Transform parent)
    {
        return GameObject.Instantiate(_resourceObjectsProvider.GetGameObject(_resourcesConfig.FruitPath)
            ,position ,Quaternion.identity, parent);
    }
}
