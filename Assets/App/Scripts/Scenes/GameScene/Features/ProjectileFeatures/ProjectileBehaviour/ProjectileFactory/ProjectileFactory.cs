using UnityEngine;

public class ProjectileFactory : IProjectileFactory
{
    private int _sortingOrder = 1;
    private readonly ResourceObjectsProvider _resourceObjectsProvider;
    private readonly ProjectileContainer _projectileContainer;
    private readonly ShadowContainer _shadowContainer;
    private readonly IDestroyTrigger _destroyTrigger;
    private readonly ResourcesConfig _resourcesConfig;
    private readonly ShadowConfig _shadowConfig;
    private readonly FruitConfig _fruitConfig;

    public ProjectileFactory(IDestroyTrigger destroyTrigger, ProjectileContainer projectileContainer, ShadowContainer shadowContainer
        , ResourceObjectsProvider resourceObjectsProvider, FruitConfig fruitConfig, ResourcesConfig resourcesConfig, ShadowConfig shadowConfig)
    {
        _destroyTrigger = destroyTrigger;
        _projectileContainer = projectileContainer;
        _shadowContainer = shadowContainer;
        _fruitConfig = fruitConfig;
        _resourceObjectsProvider = resourceObjectsProvider;
        _resourcesConfig = resourcesConfig;
        _shadowConfig = shadowConfig;
    }

    public WholeFruit CreateFruitByType(Vector2 position, FruitType fruitType)
    {
        WholeFruit wholeFruit = SpawnFruitAndConstruct(position);
        TrySetFruitsAndShadowsSprites(fruitType, wholeFruit, position);
        
        _destroyTrigger.AddDestroyTriggerListeners(wholeFruit.LeftFruitPart.transform, wholeFruit.LeftFruitPart.Shadow.transform);
        _destroyTrigger.AddDestroyTriggerListeners(wholeFruit.RightFruitPart.transform, wholeFruit.RightFruitPart.Shadow.transform);
        
        return wholeFruit;
    }

    private void TrySetFruitsAndShadowsSprites(FruitType fruitType, WholeFruit wholeFruit, Vector2 position)
    {
        float randomFloatBetween = _fruitConfig.FruitScaleRange.GetRandomFloatBetween();
        Vector2 randVector = new Vector2(randomFloatBetween, randomFloatBetween);
        
        if (_fruitConfig.FruitDictionary.TryGetValue(fruitType, out var fruitData))
        {
            SetShadowAndFruitSprites(wholeFruit.LeftFruitPart, fruitData.LeftSprite, SetShadowAndConstruct(position), randVector);
            SetShadowAndFruitSprites(wholeFruit.RightFruitPart, fruitData.RightSprite, SetShadowAndConstruct(position), randVector);
        }
    }

    private void SetShadowAndFruitSprites(FruitPart fruitPart, Sprite fruitSprite, Shadow shadow, Vector2 offsetVector)
    {
        fruitPart.SetShadow(shadow);
        fruitPart.SetSprite(fruitSprite, offsetVector, _sortingOrder++);
        shadow.SetSpriteWithOffset(fruitSprite, offsetVector, 0.2f);
        shadow.TurnIntoShadow();
    }

    private Shadow SetShadowAndConstruct(Vector2 position)
    {
        Shadow shadow = SpawnShadowWithParentZPosition(position, _shadowContainer.transform).GetComponent<Shadow>();
        shadow.Construct(_shadowConfig);
        return shadow;
    }

    private WholeFruit SpawnFruitAndConstruct(Vector2 position)
    {
        WholeFruit wholeFruit = SpawnFruitWithParentZPosition(position, _projectileContainer.transform).GetComponent<WholeFruit>();
        return wholeFruit;
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
