using UnityEngine;

public class ProjectileFactory : IProjectileFactory
{
    private int _sortingOrder = 1;
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

    public WholeFruit CreateFruitByType(Vector2 position, FruitType fruitType)
    {
        WholeFruit wholeFruit = SpawnFruitAndConstruct(position);
        
        Shadow shadow1 = SetShadowAndConstruct(position);
        Shadow shadow2 = SetShadowAndConstruct(position);

        Shadow[] shadows = new Shadow[2] { shadow1, shadow2 };
        FruitPart[] fruitParts = new FruitPart[2] { wholeFruit.LeftFruitPart, wholeFruit.RightFruitPart };
        FruitPartEnum[] fruitPartEnums = new FruitPartEnum[2] { FruitPartEnum.Left, FruitPartEnum.Right };
        
        TrySetFruitsAndShadowsSprites(fruitType, fruitPartEnums, shadows, fruitParts);
        _destroyTrigger.AddDestroyTriggerListeners(wholeFruit.LeftFruitPart.transform, wholeFruit.LeftFruitPart.Shadow.transform);
        _destroyTrigger.AddDestroyTriggerListeners(wholeFruit.RightFruitPart.transform, wholeFruit.RightFruitPart.Shadow.transform);

        return wholeFruit;
    }

    private void TrySetFruitsAndShadowsSprites(FruitType fruitType, FruitPartEnum[] fruitPartEnum, Shadow[] shadow, params FruitPart[] fruitPart)
    {
        if (_fruitConfig.FruitDictionary.TryGetValue(fruitType, out var fruitData))
            SetShadowAndFruitSprites(fruitPartEnum, fruitData, shadow, fruitPart);
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
        wholeFruit.LeftFruitPart.Construct(_coroutineRunner, _shadowConfig);
        wholeFruit.RightFruitPart.Construct(_coroutineRunner, _shadowConfig);
        return wholeFruit;
    }

    private void SetShadowAndFruitSprites(FruitPartEnum[] fruitPartEnum, FruitData fruitData, Shadow[] shadow, params FruitPart[] fruitPart)
    {
        Sprite[] spr = new Sprite[fruitPart.Length];

        for (int i = 0; i < fruitPart.Length; i++)
        {
            switch (fruitPartEnum[i])
            {
                case (FruitPartEnum.Left):
                    spr[i] = fruitData.LeftSprite;
                    break;
                case (FruitPartEnum.Right):
                    spr[i] = fruitData.RightSprite;
                    break;
            }
        }
        
        float randomFloatBetween = _fruitConfig.FruitScaleRange.GetRandomFloatBetween();

        for (int i = 0; i < fruitPart.Length; i++)
        {
            fruitPart[i].SetShadow(shadow[i]);
            Vector2 randVector = new Vector2(randomFloatBetween, randomFloatBetween);
            fruitPart[i].SetSprite(spr[i], randVector, _sortingOrder++);
            shadow[i].SetSpriteWithOffset(spr[i], randVector, 0.2f);
            shadow[i].TurnIntoShadow();
        }
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
