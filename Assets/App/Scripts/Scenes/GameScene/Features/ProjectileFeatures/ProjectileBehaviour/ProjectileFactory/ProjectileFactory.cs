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
        private readonly ProjectileConfig _projectileConfig;
        private readonly Vector2 _defaultPosition = new(100f, 100f);

        public ProjectileFactory(IDestroyTrigger destroyTrigger, ProjectileContainer.ProjectileContainer projectileContainer, ShadowContainer shadowContainer, SliceCollidersController sliceCollidersController
            , ResourceObjectsProvider resourceObjectsProvider, ParticleSystemPlayer particleSystemPlayer, ProjectileConfig projectileConfig, ResourcesConfig resourcesConfig, ShadowConfig shadowConfig
            , HealthSystem healthSystem)
        {
            _destroyTrigger = destroyTrigger;
            _projectileContainer = projectileContainer;
            _shadowContainer = shadowContainer;
            _sliceCollidersController = sliceCollidersController;
            _projectileConfig = projectileConfig;
            _resourceObjectsProvider = resourceObjectsProvider;
            _particleSystemPlayer = particleSystemPlayer;
            _resourcesConfig = resourcesConfig;
            _shadowConfig = shadowConfig;
            _healthSystem = healthSystem;
        }

        public Fruit CreateFruit(FruitType fruitType, Vector2 position, Vector2 fruitScale, Vector2 shadowScale, out Shadow shadow)
        {
            Fruit fruit = CreateFruitPart(ProjectilePartEnum.Whole, fruitType, position, fruitScale, shadowScale, out var createdShadow, out var projectileObject);
            
            fruit.DestroyNotSliced += _healthSystem.LooseLife;
            
            fruit.GetComponent<SliceObject>().Construct(
                () => CreateFruitPart(ProjectilePartEnum.Left, fruitType, _defaultPosition, projectileObject.Scale, createdShadow.Scale, out var shadowLeft, out var leftProjectile).GetComponent<ISliced>()
                ,() => CreateFruitPart(ProjectilePartEnum.Right, fruitType, _defaultPosition, projectileObject.Scale, createdShadow.Scale, out var shadowRight, out var leftProjectile).GetComponent<ISliced>()
                , fruit, _destroyTrigger);
            
            fruit.GetComponent<SliceCircleCollider>().Construct(_sliceCollidersController);
            shadow = createdShadow;
            return fruit;
        }
        
        public Bomb CreateBomb(Vector2 position, Vector2 fruitScale, Vector2 shadowScale, out Shadow shadow)
        {
            Bomb bomb = CreateBombPart(ProjectilePartEnum.Whole, position, fruitScale, shadowScale, out var createdShadow, out var projectileObject);
            
            bomb.GetComponent<SliceObject>().Construct(
                () => CreateBombPart(ProjectilePartEnum.Left, _defaultPosition, projectileObject.Scale, createdShadow.Scale, out var shadowLeft, out var rightProjectile).GetComponent<ISliced>()
                ,() => CreateBombPart(ProjectilePartEnum.Right, _defaultPosition, projectileObject.Scale, createdShadow.Scale, out var shadowRight, out var rightProjectile).GetComponent<ISliced>()
                , bomb, _destroyTrigger);
            
            bomb.GetComponent<SliceCircleCollider>().Construct(_sliceCollidersController);
            shadow = createdShadow;
            return bomb;
        }
        
        public Heart CreateHeart(Vector2 position, Vector2 fruitScale, Vector2 shadowScale, out Shadow shadow)
        {
            Heart heart = CreateHeartPart(ProjectilePartEnum.Whole, position, fruitScale, shadowScale, out var createdShadow, out var projectileObject);
            
            heart.GetComponent<SliceObject>().Construct(
                () => CreateHeartPart(ProjectilePartEnum.Left, _defaultPosition, projectileObject.Scale, createdShadow.Scale, out var shadowLeft, out var rightProjectile).GetComponent<ISliced>()
                ,() => CreateHeartPart(ProjectilePartEnum.Right, _defaultPosition, projectileObject.Scale, createdShadow.Scale, out var shadowRight, out var rightProjectile).GetComponent<ISliced>()
                , heart, _destroyTrigger);
            
            heart.GetComponent<SliceCircleCollider>().Construct(_sliceCollidersController);
            shadow = createdShadow;
            return heart;
        }


        private Fruit CreateFruitPart(ProjectilePartEnum projectilePart, FruitType fruitType, Vector2 position, Vector2 fruitScale, Vector2 shadowScale, out Shadow shadow, out ProjectileObject projectileObject)
        {
            Fruit fruit = SpawnFruit(projectilePart, position, fruitScale, _projectileContainer.transform);
            projectileObject = fruit.GetComponent<ProjectileObject>();
            shadow = SpawnShadowAndConstruct(projectileObject, position, fruitScale);
            
            if (_projectileConfig.FruitDictionary.TryGetValue(fruitType, out FruitData fruitData))
            {
                fruit.Construct(fruitData.SliceColor, _particleSystemPlayer);
                SetProjectileAndShadowSprite(projectileObject, fruitData.PartSprites[projectilePart], shadow, fruitScale, shadowScale);
            }
            
            _destroyTrigger.AddDestroyTriggerListeners(fruit.transform, shadow.transform);
            return fruit;
        }
        
        public Bomb CreateBombPart(ProjectilePartEnum projectilePart, Vector2 position, Vector2 projectileScale, Vector2 shadowScale, out Shadow shadow, out ProjectileObject projectileObject)
        {
            Bomb bomb = SpawnBomb(projectilePart, position, projectileScale, _projectileContainer.transform);
            projectileObject = bomb.GetComponent<ProjectileObject>();
            shadow = SpawnShadowAndConstruct(projectileObject, position, projectileScale);
            
            bomb.Construct(_particleSystemPlayer);
            
            if (_projectileConfig.BonusesDictionary.TryGetValue(BonusesType.Bomb, out BonusData bonusData))
            {
                SetProjectileAndShadowSprite(projectileObject, bonusData.PartSprites[projectilePart], shadow, projectileScale, shadowScale);
            }
            
            _destroyTrigger.AddDestroyTriggerListeners(bomb.transform, shadow.transform);
            return bomb;
        }
        
        public Heart CreateHeartPart(ProjectilePartEnum projectilePart, Vector2 position, Vector2 projectileScale, Vector2 shadowScale, out Shadow shadow, out ProjectileObject projectileObject)
        {
            Heart heart = SpawnHeart(projectilePart, position, projectileScale, _projectileContainer.transform);
            projectileObject = heart.GetComponent<ProjectileObject>();
            shadow = SpawnShadowAndConstruct(projectileObject, position, projectileScale);
            
            heart.Construct(_particleSystemPlayer);
            
            if (_projectileConfig.BonusesDictionary.TryGetValue(BonusesType.Bomb, out BonusData bonusData))
            {
                SetProjectileAndShadowSprite(projectileObject, bonusData.PartSprites[projectilePart], shadow, projectileScale, shadowScale);
            }
            
            _destroyTrigger.AddDestroyTriggerListeners(heart.transform, shadow.transform);
            return heart;
        }

        private Shadow SpawnShadowAndConstruct(ProjectileObject projectileObject, Vector2 position, Vector2 scale)
        {
            Shadow shadow = SpawnShadow(position, scale, _shadowContainer.transform);
            projectileObject.ShadowCloneMover.Construct(shadow.gameObject);
            projectileObject.ShadowCloneRotater.Construct(shadow.SpriteRenderer.gameObject);
            return shadow;
        }

        private void SetProjectileAndShadowSprite(ProjectileObject projectileObject, Sprite sprite, Shadow shadow, Vector2 fruitScale, Vector2 shadowScale)
        {
            Vector2 shadowVector = new Vector2(_shadowConfig.ShadowDirectionX, _shadowConfig.ShadowDirectionY) *
                                   _shadowConfig.DefaultShadowOffset;
            projectileObject.SetSpriteAndScale(sprite, fruitScale, _sortingOrder++);
            shadow.SetSpriteWithOffsetAndScale(sprite, shadowVector, shadowScale);
            shadow.TurnIntoShadow();
        }

        private Heart SpawnHeart(ProjectilePartEnum projectilePart, Vector2 position, Vector2 scale, Transform parent) =>
            SpawnObjectFromResources<Heart>(_resourcesConfig.HeartPartPath[projectilePart], position, scale, parent);
        
        private Bomb SpawnBomb(ProjectilePartEnum projectilePart, Vector2 position, Vector2 scale, Transform parent) =>
            SpawnObjectFromResources<Bomb>(_resourcesConfig.BombPartPath[projectilePart], position, scale, parent);
        
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
