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
        private readonly ProjectilesParenter _projectileParenter;
        private readonly SliceCollidersController _sliceCollidersController;
        private readonly ResourceObjectsProvider _resourceObjectsProvider;
        private readonly ParticleSystemPlayer _particleSystemPlayer;
        private readonly BonusesConfig _bonusesConfig;
        private readonly ProjectileContainer _projectileContainer;
        private readonly ShadowParenter _shadowParenter;
        private readonly IDestroyTrigger _destroyTrigger;
        private readonly ResourcesConfig _resourcesConfig;
        private readonly ShadowConfig _shadowConfig;
        private readonly HealthSystem _healthSystem;
        private readonly ProjectileConfig _projectileConfig;
        private readonly Vector2 _defaultPosition = new(100f, 100f);

        public ProjectileFactory(IDestroyTrigger destroyTrigger, ProjectilesParenter projectileParenter, ShadowParenter shadowParenter, SliceCollidersController sliceCollidersController
            , ResourceObjectsProvider resourceObjectsProvider, ParticleSystemPlayer particleSystemPlayer, ProjectileConfig projectileConfig, ResourcesConfig resourcesConfig, ShadowConfig shadowConfig
            , HealthSystem healthSystem, BonusesConfig bonusesConfig, ProjectileContainer projectileContainer)
        {
            _destroyTrigger = destroyTrigger;
            _projectileParenter = projectileParenter;
            _shadowParenter = shadowParenter;
            _sliceCollidersController = sliceCollidersController;
            _projectileConfig = projectileConfig;
            _resourceObjectsProvider = resourceObjectsProvider;
            _particleSystemPlayer = particleSystemPlayer;
            _resourcesConfig = resourcesConfig;
            _shadowConfig = shadowConfig;
            _healthSystem = healthSystem;
            _bonusesConfig = bonusesConfig;
            _projectileContainer = projectileContainer;
        }
        
        public Magnet CreateMagnet(Vector2 position, Vector2 magnetSclae, Vector2 shadowScale, out Shadow shadow)
        {
            Magnet magnet = CreateMagnetPart(ProjectilePartEnum.Whole, BonusesType.Magnet, position, magnetSclae, shadowScale, out shadow, out var projectileObject);
            magnet.GetComponent<DestroySliceObject>().Construct(magnet, _destroyTrigger);
            magnet.GetComponent<SliceCircleCollider>().Construct(_sliceCollidersController);
            return magnet;
        }

        public Fruit CreateFruit(FruitType fruitType, Vector2 position, Vector2 fruitScale, Vector2 shadowScale, out Shadow shadow)
        {
            Fruit fruit = CreateFruitPart(ProjectilePartEnum.Whole, fruitType, position, fruitScale, shadowScale, out var createdShadow, out var projectileObject);
            
            fruit.DestroyNotSliced += _healthSystem.LooseLife;
            
            fruit.GetComponent<TwoPartsSliceObject>().Construct(
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
            
            bomb.GetComponent<TwoPartsSliceObject>().Construct(
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
            
            heart.GetComponent<TwoPartsSliceObject>().Construct(
                () => CreateHeartPart(ProjectilePartEnum.Left, _defaultPosition, projectileObject.Scale, createdShadow.Scale, out var shadowLeft, out var rightProjectile).GetComponent<ISliced>()
                ,() => CreateHeartPart(ProjectilePartEnum.Right, _defaultPosition, projectileObject.Scale, createdShadow.Scale, out var shadowRight, out var rightProjectile).GetComponent<ISliced>()
                , heart, _destroyTrigger);
            
            heart.GetComponent<SliceCircleCollider>().Construct(_sliceCollidersController);
            shadow = createdShadow;
            return heart;
        }


        private Magnet CreateMagnetPart(ProjectilePartEnum projectilePart, BonusesType bonusesType, Vector2 position, Vector2 fruitScale, Vector2 shadowScale, out Shadow shadow, out ProjectileObject projectileObject)
        {
            Magnet magnet = SpawnMagnet(projectilePart, position, fruitScale, _projectileParenter.transform);
            projectileObject = magnet.GetComponent<ProjectileObject>();
            _projectileContainer.AddToDictionary(ProjectileType.Magnet, projectileObject);
            
            shadow = SpawnShadowAndConstructProjectileObject(projectileObject, projectilePart, position, fruitScale);
            
            if (_projectileConfig.BonusesDictionary.TryGetValue(bonusesType, out BonusData bonusData))
            {
                magnet.Construct(_particleSystemPlayer, _bonusesConfig);
                SetProjectileAndShadowSprite(projectileObject, bonusData.PartSprites[projectilePart], shadow, fruitScale, shadowScale);
            }
            
            _destroyTrigger.AddDestroyTriggerListeners(projectileObject);
            return magnet;
        }

        
        private Fruit CreateFruitPart(ProjectilePartEnum projectilePart, FruitType fruitType, Vector2 position, Vector2 fruitScale, Vector2 shadowScale, out Shadow shadow, out ProjectileObject projectileObject)
        {
            Fruit fruit = SpawnFruit(projectilePart, position, fruitScale, _projectileParenter.transform);
            projectileObject = fruit.GetComponent<ProjectileObject>();
            _projectileContainer.AddToDictionary(ProjectileType.Fruit, projectileObject);
            shadow = SpawnShadowAndConstructProjectileObject(projectileObject,projectilePart, position, fruitScale);
            
            if (_projectileConfig.FruitDictionary.TryGetValue(fruitType, out FruitData fruitData))
            {
                fruit.Construct(fruitData.SliceColor, _particleSystemPlayer);
                SetProjectileAndShadowSprite(projectileObject, fruitData.PartSprites[projectilePart], shadow, fruitScale, shadowScale);
            }
            
            _destroyTrigger.AddDestroyTriggerListeners(projectileObject);
            return fruit;
        }
        
        public Bomb CreateBombPart(ProjectilePartEnum projectilePart, Vector2 position, Vector2 projectileScale, Vector2 shadowScale, out Shadow shadow, out ProjectileObject projectileObject)
        {
            Bomb bomb = SpawnBomb(projectilePart, position, projectileScale, _projectileParenter.transform);
            projectileObject = bomb.GetComponent<ProjectileObject>();
            _projectileContainer.AddToDictionary(ProjectileType.Bomb, projectileObject);
            shadow = SpawnShadowAndConstructProjectileObject(projectileObject,projectilePart, position, projectileScale);
            
            bomb.Construct(_particleSystemPlayer, _healthSystem);
            
            if (_projectileConfig.BonusesDictionary.TryGetValue(BonusesType.Bomb, out BonusData bonusData))
            {
                SetProjectileAndShadowSprite(projectileObject, bonusData.PartSprites[projectilePart], shadow, projectileScale, shadowScale);
            }
            
            _destroyTrigger.AddDestroyTriggerListeners(projectileObject);
            return bomb;
        }
        
        public Heart CreateHeartPart(ProjectilePartEnum projectilePart, Vector2 position, Vector2 projectileScale, Vector2 shadowScale, out Shadow shadow, out ProjectileObject projectileObject)
        {
            Heart heart = SpawnHeart(projectilePart, position, projectileScale, _projectileParenter.transform);
            projectileObject = heart.GetComponent<ProjectileObject>();
            _projectileContainer.AddToDictionary(ProjectileType.Heart, projectileObject);
            shadow = SpawnShadowAndConstructProjectileObject(projectileObject,projectilePart, position, projectileScale);
            
            heart.Construct(_particleSystemPlayer, _healthSystem);
            
            if (_projectileConfig.BonusesDictionary.TryGetValue(BonusesType.Heart, out BonusData bonusData))
            {
                SetProjectileAndShadowSprite(projectileObject, bonusData.PartSprites[projectilePart], shadow, projectileScale, shadowScale);
            }
            
            _destroyTrigger.AddDestroyTriggerListeners(projectileObject);
            return heart;
        }

        private Shadow SpawnShadowAndConstructProjectileObject(ProjectileObject projectileObject, ProjectilePartEnum partEnum, Vector2 position, Vector2 scale)
        {
            Shadow shadow = SpawnShadow(position, scale, _shadowParenter.transform);
            projectileObject.Construct(partEnum, shadow);
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

        private Magnet SpawnMagnet(ProjectilePartEnum projectilePart, Vector2 position, Vector2 scale, Transform parent) =>
            SpawnObjectFromResources<Magnet>(_resourcesConfig.MagnetPartPath[projectilePart], position, scale, parent);
        
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
