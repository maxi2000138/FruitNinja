using App.Scripts.Scenes.GameScene.Configs;
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
        private readonly ResourceObjectsProvider _resourceObjectsProvider;
        private readonly ProjectileContainer.ProjectileContainer _projectileContainer;
        private readonly ShadowContainer _shadowContainer;
        private readonly SliceCollidersController _sliceCollidersController;
        private readonly IDestroyTrigger _destroyTrigger;
        private readonly ResourcesConfig _resourcesConfig;
        private readonly ShadowConfig _shadowConfig;
        private readonly FruitConfig _fruitConfig;

        public ProjectileFactory(IDestroyTrigger destroyTrigger, ProjectileContainer.ProjectileContainer projectileContainer, ShadowContainer shadowContainer, SliceCollidersController sliceCollidersController
            , ResourceObjectsProvider resourceObjectsProvider, FruitConfig fruitConfig, ResourcesConfig resourcesConfig, ShadowConfig shadowConfig)
        {
            _destroyTrigger = destroyTrigger;
            _projectileContainer = projectileContainer;
            _shadowContainer = shadowContainer;
            _sliceCollidersController = sliceCollidersController;
            _fruitConfig = fruitConfig;
            _resourceObjectsProvider = resourceObjectsProvider;
            _resourcesConfig = resourcesConfig;
            _shadowConfig = shadowConfig;
        }

        public Fruit CreateFruitWithShadow(FruitType fruitType, Vector2 position, Vector2 scale, out Shadow shadow)
        {
            Fruit fruit = SpawnFruitWithShadowAndConstruct(fruitType, position, scale, out shadow);
            Fruit leftPart = SpawnFruitPartWithShadowAndConstruct(ProjectileSide.Left, fruitType, position, scale, out var shadowLeft);
            Fruit rightPart = SpawnFruitPartWithShadowAndConstruct(ProjectileSide.Right, fruitType, position, scale, out var shadowRight);
            leftPart.gameObject.SetActive(false);
            shadowLeft.gameObject.SetActive(false);
            rightPart.gameObject.SetActive(false);
            shadowRight.gameObject.SetActive(false);

            
            fruit.GetComponent<SliceObject>().Construct(leftPart.GetComponent<SlicedObject>(), leftPart.GetComponent<SlicedObject>());
            fruit.GetComponent<SliceCircleCollider>().Construct(_sliceCollidersController);
            
            _destroyTrigger.AddDestroyTriggerListeners(fruit.transform, shadow.transform, leftPart.transform.parent, shadowLeft.transform, shadowRight.transform.parent, shadowRight.transform);
            return fruit;
        }

        private Fruit SpawnFruitWithShadowAndConstruct(FruitType fruitType, Vector2 position, Vector2 scale, out Shadow shadow)
        {
            Fruit fruit = SpawnFruit(position, scale, _projectileContainer.transform);
            shadow = SpawnShadow(position, scale, _shadowContainer.transform);
            SetFruitAndShadowSprites(fruit, shadow, fruitType);

            fruit.ShadowCloneMover.Construct(shadow.gameObject);
            fruit.ShadowCloneRotater.Construct(shadow.SpriteRenderer.gameObject);
            return fruit;
        }
        
        private Fruit SpawnFruitPartWithShadowAndConstruct(ProjectileSide projectileSide, FruitType fruitType, Vector2 position, Vector2 scale, out Shadow shadow)
        {
            Fruit fruit = SpawnFruitPart(position, scale, _projectileContainer.transform);
            shadow = SpawnShadow(position, scale, _shadowContainer.transform);
            SetFruitPartAndShadowSprites(projectileSide, fruit, shadow, fruitType);

            fruit.ShadowCloneMover.Construct(shadow.gameObject);
            fruit.ShadowCloneRotater.Construct(shadow.SpriteRenderer.gameObject);
            return fruit;
        }

        private void SetFruitAndShadowSprites(Fruit fruit, Shadow shadow, FruitType fruitType)
        {
            if (_fruitConfig.FruitDictionary.TryGetValue(fruitType, out FruitData fruitData))
            {
                fruit.SetSprite(fruitData.FullSprite, _sortingOrder++);
                Vector2 shadowVector = new Vector2(_shadowConfig.ShadowDirectionX, _shadowConfig.ShadowDirectionY) * _shadowConfig.DefaultShadowOffset;
                shadow.SetSpriteWithOffset(fruitData.FullSprite, shadowVector);
                shadow.TurnIntoShadow();
            }
        }
        
        private void SetFruitPartAndShadowSprites(ProjectileSide projectileSide, Fruit fruit, Shadow shadow, FruitType fruitType)
        {
            if (_fruitConfig.FruitDictionary.TryGetValue(fruitType, out FruitData fruitData))
            {
                Vector2 shadowVector = new Vector2(_shadowConfig.ShadowDirectionX, _shadowConfig.ShadowDirectionY) * _shadowConfig.DefaultShadowOffset;
                if (projectileSide == ProjectileSide.Left)
                {
                    fruit.SetSprite(fruitData.LeftSprite, _sortingOrder++);
                    shadow.SetSpriteWithOffset(fruitData.LeftSprite, shadowVector);
                }
                else
                {
                    fruit.SetSprite(fruitData.RightSprite, _sortingOrder++);
                    shadow.SetSpriteWithOffset(fruitData.RightSprite, shadowVector);
                }
                
                shadow.TurnIntoShadow();
            }
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
