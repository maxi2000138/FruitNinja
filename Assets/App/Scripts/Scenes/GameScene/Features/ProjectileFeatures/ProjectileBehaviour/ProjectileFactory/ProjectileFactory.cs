using UnityEngine;

public class ProjectileFactory : IProjectileFactory
{
    private readonly ResourceObjectsProvider _resourceObjectsProvider;
    private readonly ProjectileContainer _projectileContainer;
    private readonly ShadowContainer _shadowContainer;
    private readonly ICoroutineRunner _coroutineRunner;
    private readonly IDestroyTrigger _destroyTrigger;
    private readonly ResourcesConfig _resourcesConfig;
    private readonly ShadowConfig _shadowConfig;
    private readonly FruitConfig _fruitConfig;

    public ProjectileFactory(IDestroyTrigger destroyTrigger, ProjectileContainer projectileContainer, ShadowContainer shadowContainer, ICoroutineRunner coroutineRunner
        , ResourceObjectsProvider resourceObjectsProvider, FruitConfig fruitConfig, ResourcesConfig resourcesConfig, ShadowConfig shadowConfig)
    {
        _destroyTrigger = destroyTrigger;
        _projectileContainer = projectileContainer;
        _shadowContainer = shadowContainer;
        _coroutineRunner = coroutineRunner;
        _fruitConfig = fruitConfig;
        _resourceObjectsProvider = resourceObjectsProvider;
        _resourcesConfig = resourcesConfig;
        _shadowConfig = shadowConfig;
    }

    public Fruit CreateFruitByType(Vector2 position, FruitType fruitType)
    {
        Fruit fruit = SpawnFruitAndConstruct(position);
        Shadow shadow = SetShadowAndConstruct(position);
        TrySetFruitsAndShadowsSprites(fruitType, fruit, shadow);
        _destroyTrigger.AddDestroyTriggerListeners(fruit.transform, fruit.Shadow.transform);
        return fruit;
    }

    private void TrySetFruitsAndShadowsSprites(FruitType fruitType, Fruit fruit, Shadow shadow)
    {
        if (_fruitConfig.FruitDictionary.TryGetValue(fruitType, out var fruitData))
            SetShadowAndFruitSprites(fruit, fruitData, shadow);
    }

    private Shadow SetShadowAndConstruct(Vector2 position)
    {
        Shadow shadow = SpawnShadowWithParentZPosition(position, _shadowContainer.transform).GetComponent<Shadow>();
        shadow.Construct(_shadowConfig);
        return shadow;
    }

    private Fruit SpawnFruitAndConstruct(Vector2 position)
    {
        Fruit fruit = SpawnFruitWithParentZPosition(position, _projectileContainer.transform).GetComponent<Fruit>();
        fruit.Construct(_coroutineRunner, _shadowConfig);
        return fruit;
    }

    private void SetShadowAndFruitSprites(Fruit fruit, FruitData fruitData, Shadow shadow)
    {
        fruit.SetShadow(shadow);
        float randomFloatBetween = _fruitConfig.FruitScaleRange.GetRandomFloatBetween();
        Vector2 randVector = new Vector2(randomFloatBetween, randomFloatBetween);
        fruit.SetSprite(fruitData.Sprite, randVector);
        shadow.SetSpriteWithOffset(fruitData.Sprite, randVector, 0.2f);
        shadow.TurnIntoShadow();
    }

    private GameObject SpawnFruitWithParentZPosition(Vector2 position, Transform parent)
    {
        Vector3 worldPosition = new Vector3(position.x, position.y, parent.position.z);
        return GameObject.Instantiate(_resourceObjectsProvider.GetGameObject(_resourcesConfig.FruitPath)
            ,worldPosition ,Quaternion.identity, parent);
    }
    
    private GameObject SpawnShadowWithParentZPosition(Vector2 position, Transform parent)
    {
        Vector3 worldPosition = new Vector3(position.x, position.y, parent.position.z);
        return GameObject.Instantiate(_resourceObjectsProvider.GetGameObject(_resourcesConfig.ShadowPath)
            ,worldPosition ,Quaternion.identity, parent);
    }
}
