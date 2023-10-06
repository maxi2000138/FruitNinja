using UnityEngine;

public class ProjectileFactory : IProjectileFactory
{
    private readonly ResourceObjectsProvider _resourceObjectsProvider;
    private readonly ProjectileContainer _projectileContainer;
    private readonly ShadowContainer _shadowContainer;
    private readonly IDestroyTrigger _destroyTrigger;
    private readonly ResourcesConfig _resourcesConfig;
    private readonly ShadowConfig _shadowConfig;
    private readonly FruitConfig _fruitConfig;

    public ProjectileFactory(IDestroyTrigger destroyTrigger, ProjectileContainer projectileContainer, ShadowContainer shadowContainer, ResourceObjectsProvider resourceObjectsProvider,
        FruitConfig fruitConfig, ResourcesConfig resourcesConfig, ShadowConfig shadowConfig)
    {
        _destroyTrigger = destroyTrigger;
        _projectileContainer = projectileContainer;
        _shadowContainer = shadowContainer;
        _fruitConfig = fruitConfig;
        _resourceObjectsProvider = resourceObjectsProvider;
        _resourcesConfig = resourcesConfig;
        _shadowConfig = shadowConfig;
    }

    public Fruit CreateFruitByType(Vector2 position, FruitType fruitType)
    {
        Fruit fruit = SpawnFruit(position, _projectileContainer.transform).GetComponent<Fruit>();
        Shadow shadow = SpawnShadow(position, _shadowContainer.transform).GetComponent<Shadow>();
        if (_fruitConfig.FruitDictionary.TryGetValue(fruitType, out var fruitData))
        {
            fruit.SetSprite(fruitData.Sprite, fruitData.SpriteScale);
            fruit.SetShadow(shadow);
            shadow.Construct(_shadowConfig);
            shadow.TurnIntoShadow();
            shadow.SetSpriteWithOffset(fruitData.Sprite, fruitData.SpriteScale);
        }
        
        _destroyTrigger.AddDestroyTriggerListeners(fruit.transform, fruit.Shadow.transform);
        return fruit;
    }

    private GameObject SpawnFruit(Vector2 position, Transform parent)
    {
        Vector3 worldPosition = new Vector3(position.x, position.y, parent.position.z);
        return GameObject.Instantiate(_resourceObjectsProvider.GetGameObject(_resourcesConfig.FruitPath)
            ,worldPosition ,Quaternion.identity, parent);
    }
    
    private GameObject SpawnShadow(Vector2 position, Transform parent)
    {
        Vector3 worldPosition = new Vector3(position.x, position.y, parent.position.z);
        return GameObject.Instantiate(_resourceObjectsProvider.GetGameObject(_resourcesConfig.ShadowPath)
            ,worldPosition ,Quaternion.identity, parent);
    }
}
