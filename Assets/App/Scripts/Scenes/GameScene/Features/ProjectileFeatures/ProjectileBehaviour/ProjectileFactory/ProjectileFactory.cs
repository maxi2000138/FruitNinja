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
        private readonly FruitConfig _fruitConfig;

        public ProjectileFactory(IDestroyTrigger destroyTrigger, ProjectileContainer.ProjectileContainer projectileContainer, ShadowContainer shadowContainer, SliceCollidersController sliceCollidersController
            , ResourceObjectsProvider resourceObjectsProvider, ParticleSystemPlayer particleSystemPlayer, FruitConfig fruitConfig, ResourcesConfig resourcesConfig, ShadowConfig shadowConfig)
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
        }

        public Fruit CreateFruitWithShadow(FruitType fruitType, Vector2 position, Vector2 scale, out Shadow shadow)
        {
            Fruit fruit = SpawnFruitAndShadowAndConstruct(position, scale, out var fruitShadow);
            fruit.Construct(this, fruitType);


            if (_fruitConfig.FruitDictionary.TryGetValue(fruitType, out FruitData spriteData))
            {
                SetFruitAndShadowSprite(fruit, spriteData.FullSprite, fruitShadow, scale, scale);
            }
            
            fruit.GetComponent<SliceObject>().Construct(() => CreateFruitLeftPart(fruitType, fruit.Scale, fruitShadow.Scale).GetComponent<ISliced>()
                ,() => CreateFruitRightPart(fruitType, fruit.Scale, fruitShadow.Scale).GetComponent<ISliced>(), _particleSystemPlayer, _destroyTrigger);
            
            fruit.GetComponent<SliceCircleCollider>().Construct(_sliceCollidersController);
            
            _destroyTrigger.AddDestroyTriggerListeners(fruit.transform, fruitShadow.transform);
            shadow = fruitShadow;
            return fruit;
        }

        public Fruit  CreateFruitLeftPart(FruitType fruitType, Vector2 fruitScale, Vector2 shadowScale)
        {
            Fruit leftPart = SpawnFruitPartAndShadowAndConstruct(out var leftFruitShadow);
            
            if (_fruitConfig.FruitDictionary.TryGetValue(fruitType, out FruitData spriteData))
            {
                SetFruitAndShadowSprite(leftPart, spriteData.LeftSprite, leftFruitShadow, fruitScale, shadowScale);
            }
            
            _destroyTrigger.AddDestroyTriggerListeners(leftPart.transform, leftFruitShadow.transform);
            return leftPart;
        }
        
        public Fruit CreateFruitRightPart(FruitType fruitType, Vector2 fruitScale, Vector2 shadowScale)
        {
            Fruit rightPart  = SpawnFruitPartAndShadowAndConstruct(out var rightFruitShadow);
            
            if (_fruitConfig.FruitDictionary.TryGetValue(fruitType, out FruitData spriteData))
            {
                SetFruitAndShadowSprite(rightPart, spriteData.RightSprite, rightFruitShadow, fruitScale, shadowScale);
            }
            
            _destroyTrigger.AddDestroyTriggerListeners(rightPart.transform, rightFruitShadow.transform);
            return rightPart;
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

        private Fruit SpawnFruitAndShadowAndConstruct(Vector2 position, Vector2 scale, out Shadow shadow)
        {
            Fruit fruit = SpawnFruit(position, scale, _projectileContainer.transform);
            SpawnShadowAndConstruct(position, scale, out shadow, fruit);
            return fruit;
        }

        private Fruit SpawnFruitPartAndShadowAndConstruct(out Shadow leftShadow)
        {
            Fruit leftFruit = SpawnFruitPart(new Vector2(-10f,-10f), new Vector2(1f,1f), _projectileContainer.transform);
            SpawnShadowAndConstruct(new Vector2(-10f,-10f), new Vector2(1f, 1f), out leftShadow, leftFruit);
            return leftFruit;
        }

        private void SpawnShadowAndConstruct(Vector2 position, Vector2 scale, out Shadow shadow, Fruit fruit1)
        {
            shadow = SpawnShadow(position, scale, _shadowContainer.transform);
            fruit1.ShadowCloneMover.Construct(shadow.gameObject);
            fruit1.ShadowCloneRotater.Construct(shadow.SpriteRenderer.gameObject);
        }


        private Fruit SpawnFruit(Vector2 position, Vector2 scale, Transform parent)
        {
            return SpawnObjectFromResources<Fruit>(_resourcesConfig.FruitPath, position, scale, parent);
        }
        
        private Fruit SpawnFruitPart(Vector2 position, Vector2 scale, Transform parent)
        {
            return SpawnObjectFromResources<Fruit>(_resourcesConfig.FruitPartPath, position, scale, parent);
        }
    
        private Shadow SpawnShadow(Vector2 position, Vector2 scale, Transform parent)
        {
            return SpawnObjectFromResources<Shadow>(_resourcesConfig.ShadowPath, position, scale, parent);
        }

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
