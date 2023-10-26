using System;
using App.Scripts.ListExtensions;
using App.Scripts.Scenes.GameScene.Configs;
using App.Scripts.Scenes.GameScene.Features.CameraFeatures.ScreenSettingsProvider;
using App.Scripts.Scenes.GameScene.Features.InputFeatures;
using App.Scripts.Scenes.GameScene.Features.ParticleFeatures;
using App.Scripts.Scenes.GameScene.Features.PhysicsFeatures.ColliderFeatures;
using App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.FruitFeatures.Enum;
using App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.FruitFeatures.Fruit;
using App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.ProjectileBehaviour.ProjectileDestroyer.DestroyTrigger;
using App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.ShadowFeatures;
using App.Scripts.Scenes.GameScene.Features.ProjectileFeatures.SharedFeatures;
using App.Scripts.Scenes.GameScene.Features.ResourceFeatures;
using App.Scripts.Scenes.GameScene.Features.SpawnAreaFeatures;
using UnityEngine;
using Object = UnityEngine.Object;

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
        private readonly Slicer _slicer;
        private readonly TimeScaleService _timeScaleService;
        private readonly FrozerService _frozerService;
        private readonly ScreenSettingsProvider _screenSettingsProvider;
        private readonly SpawnConfig _spawnConfig;
        private readonly ShadowParenter _shadowParenter;
        private readonly IDestroyTrigger _destroyTrigger;
        private readonly ResourcesConfig _resourcesConfig;
        private readonly ShadowConfig _shadowConfig;
        private readonly HealthSystem _healthSystem;
        private readonly ProjectileConfig _projectileConfig;

        public ProjectileFactory(IDestroyTrigger destroyTrigger, ProjectilesParenter projectileParenter, ShadowParenter shadowParenter
            , SliceCollidersController sliceCollidersController, ResourceObjectsProvider resourceObjectsProvider, ParticleSystemPlayer particleSystemPlayer
            , ProjectileConfig projectileConfig, ResourcesConfig resourcesConfig, ShadowConfig shadowConfig, HealthSystem healthSystem
            , BonusesConfig bonusesConfig, ProjectileContainer projectileContainer, Slicer slicer, TimeScaleService timeScaleService
            , FrozerService frozerService, ScreenSettingsProvider screenSettingsProvider, SpawnConfig _spawnConfig)
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
            _slicer = slicer;
            _timeScaleService = timeScaleService;
            _frozerService = frozerService;
            _screenSettingsProvider = screenSettingsProvider;
            this._spawnConfig = _spawnConfig;
        }


        public ProjectileObject SpawnProjectileByTypeAndAreaData(SpawnAreaData areaData, ProjectileType projectileType, out Vector2 position, out Vector2 scale, out Shadow shadow)
        {
            GetRandomScaleAndPositionInConfigRange(areaData, projectileType,_projectileConfig ,out position, out scale);
            ProjectileObject projectileObject = null;
            shadow = null; 
            switch (projectileType)
            {
                case(ProjectileType.Fruit):
                    var type = _spawnConfig.ActiveFruitTypes.GetRandomItem();
                    projectileObject = CreateFruit(type, position, scale, scale, out shadow).GetComponent<ProjectileObject>();
                    break;
                case(ProjectileType.Bomb):
                    projectileObject = CreateBomb(position, scale, scale, out shadow).GetComponent<ProjectileObject>();
                    break;                    
                case(ProjectileType.Heart):
                    projectileObject = CreateHeart(position, scale, scale, out shadow).GetComponent<ProjectileObject>();
                    break;       
                case(ProjectileType.Magnet):
                    projectileObject = CreateMagnet(position, scale, scale, out shadow).GetComponent<ProjectileObject>();
                    break;    
                case(ProjectileType.Brick):
                    projectileObject = CreateBrick(position, scale, scale, out shadow).GetComponent<ProjectileObject>();
                    break;    
                case(ProjectileType.Ice):
                    projectileObject = CreateIce(position, scale, scale, out shadow).GetComponent<ProjectileObject>();
                    break;    
            }

            return projectileObject;
        }
        

        public Ice CreateIce(Vector2 position, Vector2 iceScale, Vector2 shadowScale, out Shadow shadow)
        {
            Ice ice = InstantiateIceAndConstruct(ProjectilePartEnum.Whole, position, iceScale, shadowScale, out shadow);
            ice.GetComponent<DestroySliceObject>().Construct(_destroyTrigger);
            return ice;
        }

        public Brick CreateBrick(Vector2 position, Vector2 magnetScale, Vector2 shadowScale, out Shadow shadow)
        {
            Brick brick = InstantiateBrickAndConstruct(ProjectilePartEnum.Whole, position, magnetScale, shadowScale, out shadow);
            return brick;
        }
        
        public Magnet CreateMagnet(Vector2 position, Vector2 magnetSclae, Vector2 shadowScale, out Shadow shadow)
        {
            Magnet magnet = InstantiateMagnetAndConstruct(ProjectilePartEnum.Whole, position, magnetSclae, shadowScale, out shadow);
            magnet.GetComponent<DestroySliceObject>().Construct(_destroyTrigger);
            return magnet;
        }

        public Fruit CreateFruit(FruitType fruitType, Vector2 position, Vector2 fruitScale, Vector2 shadowScale, out Shadow shadow)
        {
            Fruit fruit = InstantiateFruitAndConstruct(ProjectilePartEnum.Whole, fruitType, position, fruitScale, shadowScale, out shadow);
            fruit.DestroyNotSliced += _healthSystem.LooseLife;
            
            fruit.GetComponent<TwoPartsSliceObject>().Construct(
                () => InstantiateFruitAndConstruct(ProjectilePartEnum.Left, fruitType, position, fruitScale, shadowScale, out _).GetComponent<ISliced>()
                ,() => InstantiateFruitAndConstruct(ProjectilePartEnum.Right, fruitType, position, fruitScale, shadowScale, out _).GetComponent<ISliced>()
                , _destroyTrigger);
            
            return fruit;
        }
        
        public Bomb CreateBomb(Vector2 position, Vector2 fruitScale, Vector2 shadowScale, out Shadow shadow)
        {
            Bomb bomb = InstantiateBombAndConstruct(ProjectilePartEnum.Whole, position, fruitScale, shadowScale, out shadow);

            bomb.GetComponent<TwoPartsSliceObject>().Construct(
                () => InstantiateBombAndConstruct(ProjectilePartEnum.Left, position, fruitScale, shadowScale, out _).GetComponent<ISliced>(), 
                () => InstantiateBombAndConstruct(ProjectilePartEnum.Right, position, fruitScale, shadowScale, out _).GetComponent<ISliced>()
                , _destroyTrigger);
            
            return bomb;
        }
        
        public Heart CreateHeart(Vector2 position, Vector2 fruitScale, Vector2 shadowScale, out Shadow shadow)
        {
            Heart heart = InstantiateHeartAndConstruct(ProjectilePartEnum.Whole, position, fruitScale, shadowScale, out shadow);

            heart.GetComponent<TwoPartsSliceObject>().Construct(
                () => InstantiateHeartAndConstruct(ProjectilePartEnum.Left, position, fruitScale, shadowScale, out _).GetComponent<ISliced>(), 
                () => InstantiateHeartAndConstruct(ProjectilePartEnum.Right, position, fruitScale, shadowScale, out _).GetComponent<ISliced>(), _destroyTrigger);
            
            return heart;
        }
        
        private Fruit InstantiateFruitAndConstruct(ProjectilePartEnum projectilePartEnum, FruitType fruitType, Vector2 position,
            Vector2 fruitScale, Vector2 shadowScale, out Shadow createdShadow)
        {
            Fruit fruit = CreatePart<Fruit>(ProjectileType.Fruit, projectilePartEnum,
                _projectileConfig.FruitDictionary[fruitType].SpriteData, position, fruitScale, shadowScale,
                out createdShadow);
            
            _projectileContainer.AddToDictionary(ProjectileType.Fruit, fruit.GetComponent<ProjectileObject>());
            fruit.Construct(_projectileConfig.FruitDictionary[fruitType].SliceColor, _particleSystemPlayer);
            return fruit;
        }
        private Ice InstantiateIceAndConstruct(ProjectilePartEnum projectilePartEnum, Vector2 position,
            Vector2 fruitScale, Vector2 shadowScale, out Shadow createdShadow)
        {
            Ice ice = CreatePart<Ice>(ProjectileType.Ice, projectilePartEnum,
                _projectileConfig.BonusesDictionary[BonusesType.Ice].SpriteData, position, fruitScale, shadowScale,
                out createdShadow);
            
            ice.Construct(_particleSystemPlayer, _frozerService,_bonusesConfig);
            return ice;
        }

        
        private Brick InstantiateBrickAndConstruct(ProjectilePartEnum projectilePartEnum, Vector2 position,
            Vector2 fruitScale, Vector2 shadowScale, out Shadow createdShadow)
        {
            Brick brick = CreatePart<Brick>(ProjectileType.Brick, projectilePartEnum,
                _projectileConfig.BonusesDictionary[BonusesType.Brick].SpriteData, position, fruitScale, shadowScale,
                out createdShadow);
            
            brick.Construct(_slicer, _particleSystemPlayer);
            return brick;
        }
        
        private Magnet InstantiateMagnetAndConstruct(ProjectilePartEnum projectilePartEnum, Vector2 position,
            Vector2 fruitScale, Vector2 shadowScale, out Shadow createdShadow)
        {
            Magnet magnet = CreatePart<Magnet>(ProjectileType.Magnet, projectilePartEnum,
                _projectileConfig.BonusesDictionary[BonusesType.Magnet].SpriteData, position, fruitScale, shadowScale,
                out createdShadow);
            
            magnet.Construct(_particleSystemPlayer, _bonusesConfig);
            return magnet;
        }
        
        private Bomb InstantiateBombAndConstruct(ProjectilePartEnum projectilePartEnum, Vector2 position,
            Vector2 fruitScale, Vector2 shadowScale, out Shadow createdShadow)
        {
            Bomb bomb = CreatePart<Bomb>(ProjectileType.Bomb, projectilePartEnum,
                _projectileConfig.BonusesDictionary[BonusesType.Bomb].SpriteData, position, fruitScale, shadowScale,
                out createdShadow);
            
            bomb.Construct(_particleSystemPlayer, _healthSystem);
            return bomb;
        }


        private Heart InstantiateHeartAndConstruct(ProjectilePartEnum projectilePartEnum, Vector2 position,
            Vector2 fruitScale, Vector2 shadowScale, out Shadow createdShadow)
        {
            Heart heart = CreatePart<Heart>(ProjectileType.Heart, projectilePartEnum,
                _projectileConfig.BonusesDictionary[BonusesType.Heart].SpriteData, position, fruitScale, shadowScale,
                out createdShadow);
            
            heart.Construct(_particleSystemPlayer, _healthSystem);
            return heart;
        }
        
        private T CreatePart<T>(ProjectileType projectileType, ProjectilePartEnum projectilePart, SpriteData spriteData, Vector2 position,
            Vector2 fruitScale, Vector2 shadowScale, out Shadow shadow)
            where T : MonoBehaviour
        {
            T part = SpawnProjectile<T>(projectileType, projectilePart, position, fruitScale, _projectileParenter.transform);
            ProjectileObject projectileObject = part.GetComponent<ProjectileObject>();
            _projectileContainer.AddToDictionary(ProjectileType.Magnet, projectileObject);
            shadow = SpawnShadowAndConstructProjectileObject(projectileObject, projectilePart, position, fruitScale);
            
            if(projectilePart == ProjectilePartEnum.Whole)
                part.GetComponent<SliceCircleCollider>().Construct(_sliceCollidersController);
            
            SetProjectileAndShadowSprite(projectileObject, spriteData.PartSprites[projectilePart], shadow, fruitScale, shadowScale);
            _destroyTrigger.AddDestroyTriggerListeners(projectileObject);
            return part;
        }

        private Shadow SpawnShadowAndConstructProjectileObject(ProjectileObject projectileObject, ProjectilePartEnum partEnum, Vector2 position, Vector2 scale)
        {
            Shadow shadow = SpawnShadow(position, scale, _shadowParenter.transform);
            projectileObject.Construct(partEnum, shadow, _timeScaleService);
            projectileObject.ShadowCloneMover.Construct(shadow.gameObject);
            projectileObject.ShadowCloneRotater.Construct(shadow.SpriteRenderer.gameObject);
            return shadow;
        }

        private void SetProjectileAndShadowSprite(ProjectileObject projectileObject, Sprite sprite, Shadow shadow, Vector2 fruitScale, Vector2 shadowScale)
        {
            Vector2 shadowVector = new Vector2(_shadowConfig.ShadowDirectionX, _shadowConfig.ShadowDirectionY) *
                                   _shadowConfig.DefaultShadowOffset;
            projectileObject.SetSpriteAndScale(sprite, fruitScale, _sortingOrder++);
            shadow.Construct(_timeScaleService);
            shadow.SetSpriteWithOffsetAndScale(sprite, shadowVector, shadowScale);
            shadow.TurnIntoShadow();
        }
        
        private void GetRandomScaleAndPositionInConfigRange(SpawnAreaData areaData, ProjectileType projectileType, ProjectileConfig projectileConfig, out Vector2 position, out Vector2 scale)
        {
            position = _screenSettingsProvider
                .ViewportToWorldPosition((areaData.ViewportLeftPosition, areaData.ViewportRightPosition)
                    .GetRandomPointBetween());
            float randomScaleValue = projectileConfig.ProjectileScales[projectileType].Scale.GetRandomBound();
            scale = new Vector2(randomScaleValue, randomScaleValue);
        }

        private T SpawnProjectile<T>(ProjectileType projectileType, ProjectilePartEnum projectilePart, Vector2 position, Vector2 scale, Transform parent) where T : MonoBehaviour => 
            SpawnObjectFromResources<T>(_resourcesConfig.PartPathes[projectileType][projectilePart], position, scale, parent);

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
