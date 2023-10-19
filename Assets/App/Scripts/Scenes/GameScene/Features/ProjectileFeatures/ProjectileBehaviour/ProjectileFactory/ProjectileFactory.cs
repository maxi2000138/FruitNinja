using App.Scripts.Scenes.GameScene.Configs;
using App.Scripts.Scenes.GameScene.Features.ParticleFeatures;
using App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ColliderFeatures;
using App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.FruitFeatures.Enum;
using App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.FruitFeatures.Fruit;
using App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.ProjectileBehaviour.ProjectileDestroyer.DestroyTrigger;
using App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.ShadowFeatures;
using App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.SharedFeatures;
using App.Scripts.Scenes.GameScene.Features.ResourceFeatures;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.ProjectileBehaviour.ProjectileFactory
{
    public class ProjectileFactory : IProjectileFactory
    {
        private int _sortingOrder = 1;
        private readonly ProjectileContainer.ProjectileContainer _projectileContainer;
        private readonly SliceCollidersController _sliceCollidersController;
        private readonly ResourceObjectsProvider _resourceObjectsProvider;
        private readonly ParticleSystemPlayer _particleSystemPlayer;
        private readonly ShadowContainer _shadowContainer;
        private readonly IDestroyTrigger _destroyTrigger;
        private readonly ResourcesConfig _resourcesConfig;
        private readonly ShadowConfig _shadowConfig;
        private readonly HealthSystem _healthSystem;
        private readonly FruitConfig _fruitConfig;
        private readonly Vector2 _defaultPosition = new(100f, 100f);

        public ProjectileFactory(IDestroyTrigger destroyTrigger, ProjectileContainer.ProjectileContainer projectileContainer, ShadowContainer shadowContainer, SliceCollidersController sliceCollidersController
            , ResourceObjectsProvider resourceObjectsProvider, ParticleSystemPlayer particleSystemPlayer, FruitConfig fruitConfig, ResourcesConfig resourcesConfig, ShadowConfig shadowConfig
            , HealthSystem healthSystem)
        {
            _destroyTrigger = destroyTrigger;
            _projectileContainer = projectileContainer;
            _shadowContainer = shadowContainer;
            _sliceCollidersController = sliceCollidersController;
            _fruitConfig = fruitConfig;
            _resourceObjectsProvider = resourceObjectsProvider;
            _particleSystemPlayer = particleSystemPlayer;
            _resourcesConfig = resourcesConfig;
            _shadowConfig = shadowConfig;
            _healthSystem = healthSystem;
        }

        public Fruit CreateFruitWithShadow(FruitType fruitType, Vector2 position, Vector2 fruitScale, Vector2 shadowScale, out Shadow shadow)
        {
            Fruit fruit = CreateFruitPart(ProjectilePartEnum.Whole, fruitType, position, fruitScale, shadowScale, out var createdShadow);
            fruit.DestroyNotSliced += _healthSystem.LooseLife;
            fruit.GetComponent<SliceObject>().Construct(
                () => CreateFruitPart(ProjectilePartEnum.Left, fruitType, _defaultPosition, fruit.Scale, createdShadow.Scale, out var shadowLeft).GetComponent<ISliced>()
                ,() => CreateFruitPart(ProjectilePartEnum.Right, fruitType, _defaultPosition, fruit.Scale, createdShadow.Scale, out var shadowRight).GetComponent<ISliced>()
                , fruit, _destroyTrigger);
            fruit.GetComponent<SliceCircleCollider>().Construct(_sliceCollidersController);
            shadow = createdShadow;
            return fruit;
        }


        public Fruit  CreateFruitPart(ProjectilePartEnum projectilePart, FruitType fruitType, Vector2 position, Vector2 fruitScale, Vector2 shadowScale, out Shadow shadow)
        {
            Fruit fruit = SpawnFruitAndShadowAndConstruct(projectilePart, position, fruitScale, out var fruitShadow);
            if (_fruitConfig.FruitDictionary.TryGetValue(fruitType, out FruitData fruitData))
            {
                fruit.Construct(fruitData.SliceColor, _particleSystemPlayer);
                SetFruitAndShadowSprite(fruit, fruitData.PartSprites[projectilePart], fruitShadow, fruitScale, shadowScale);
            }
            
            _destroyTrigger.AddDestroyTriggerListeners(fruit.transform, fruitShadow.transform);
            shadow = fruitShadow;
            return fruit;
        }
         
        
        private void SetFruitAndShadowSprite(Fruit leftPart, Sprite sprite, Shadow shadow, Vector2 fruitScale, Vector2 shadowScale)
        {
            Vector2 shadowVector = new Vector2(_shadowConfig.ShadowDirectionX, _shadowConfig.ShadowDirectionY) *
                                   _shadowConfig.DefaultShadowOffset;
            leftPart.SetSprite(sprite, _sortingOrder++);
            leftPart.transform.localScale = fruitScale;
            shadow.transform.localScale = shadowScale;
            shadow.SetSpriteWithOffset(sprite, shadowVector);
            shadow.TurnIntoShadow();
        }

        private Fruit SpawnFruitAndShadowAndConstruct(ProjectilePartEnum projectilePart, Vector2 position, Vector2 scale, out Shadow shadow)
        {
            Fruit fruit = SpawnFruit(projectilePart, position, scale, _projectileContainer.transform);
            shadow = SpawnShadow(position, scale, _shadowContainer.transform);
            fruit.ShadowCloneMover.Construct(shadow.gameObject);
            fruit.ShadowCloneRotater.Construct(shadow.SpriteRenderer.gameObject);
            return fruit;
        }
        
        private Fruit SpawnFruit(ProjectilePartEnum projectilePart, Vector2 position, Vector2 scale, Transform parent) => 
            SpawnObjectFromResources<Fruit>(_resourcesConfig.FruitsPartPath[projectilePart], position, scale, parent);

        private Shadow SpawnShadow(Vector2 position, Vector2 scale, Transform parent) => 
            SpawnObjectFromResources<Shadow>(_resourcesConfig.ShadowPath, position, scale, parent);

        private T SpawnObjectFromResources<T>(string resourcePath, Vector2 position, Vector2 scale, Transform parent) where T : MonoBehaviour
        {
            Vector3 worldPosition = new Vector3(position.x, position.y, parent.position.z);
            T spawnedObject = Object.Instantiate(_resourceObjectsProvider.GetObject<T>(resourcePath)
                ,worldPosition ,Quaternion.identity, parent);
            spawnedObject.transform.localScale = scale;
            return spawnedObject;
        }
    }
}
