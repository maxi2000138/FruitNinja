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
        private readonly Freezer _freezer;
        private readonly ScreenSettingsProvider _screenSettingsProvider;
        private readonly SpawnConfig _spawnConfig;
        private readonly Shooter.ProjectileShooter.Shooter _shooter;
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
            , Freezer freezer, ScreenSettingsProvider screenSettingsProvider, SpawnConfig _spawnConfig, Shooter.ProjectileShooter.Shooter shooter)
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
            _freezer = freezer;
            _screenSettingsProvider = screenSettingsProvider;
            this._spawnConfig = _spawnConfig;
            _shooter = shooter;
        }


        public ProjectileObject SpawnProjectileByType(ProjectileType projectileType, Vector2 position, Vector2 scale, out Shadow shadow, Transform parent = null)
        {
            ProjectileObject projectileObject = null;
            shadow = null; 
            switch (projectileType)
            {
                case(ProjectileType.Fruit):
                    var type = _spawnConfig.ActiveFruitTypes.GetRandomItem();
                    projectileObject = CreateFruit(type, position, scale, scale, out shadow, parent).GetComponent<ProjectileObject>();
                    break;
                case(ProjectileType.Bomb):
                    projectileObject = CreateBomb(position, scale, scale, out shadow, parent).GetComponent<ProjectileObject>();
                    break;                    
                case(ProjectileType.Heart):
                    projectileObject = CreateHeart(position, scale, scale, out shadow, parent).GetComponent<ProjectileObject>();
                    break;    
                case(ProjectileType.Magnet):
                    projectileObject = CreateMagnet(position, scale, scale, out shadow, parent).GetComponent<ProjectileObject>();
                    break;    
                case(ProjectileType.Brick):
                    projectileObject = CreateBrick(position, scale, scale, out shadow, parent).GetComponent<ProjectileObject>();
                    break;    
                case(ProjectileType.Ice):
                    projectileObject = CreateIce(position, scale, scale, out shadow, parent).GetComponent<ProjectileObject>();
                    break;  
                case(ProjectileType.StringBag):
                    projectileObject = CreateStringBag(position, scale, scale, out shadow, parent).GetComponent<ProjectileObject>();
                    break;  
                case(ProjectileType.Mimik):
                    projectileObject = CreateMimik(position, scale, scale, out shadow, parent).GetComponent<ProjectileObject>();
                    break;  
            }

            return projectileObject;
        }
        

        public ProjectileObject CreateMimik(Vector2 position, Vector2 iceScale, Vector2 shadowScale, out Shadow shadow, Transform parent = null)
        {
            ProjectileType type = ProjectileType.Mimik;
            while(type == ProjectileType.Mimik)
                type = _spawnConfig.ActiveProjectileTypes.GetRandomItem();
            
            GetRandomScaleInConfigRange(type, _projectileConfig, out Vector2 scale);
            ProjectileObject projectileObject = SpawnProjectileByType(type, position, scale , out shadow, parent);
            MimikController mimikController = new MimikController(this, _shooter, _destroyTrigger
                , projectileObject, _bonusesConfig, _spawnConfig, _projectileConfig);
            mimikController.StartMimikBehaviour();
            return projectileObject;
        }
        
        public StringBag CreateStringBag(Vector2 position, Vector2 iceScale, Vector2 shadowScale, out Shadow shadow, Transform parent = null)
        {
            StringBag stringBag = InstantiateStringBagAndConstruct(ProjectilePartEnum.Whole, position, iceScale, shadowScale, out shadow, parent);
            stringBag.GetComponent<TwoPartsSliceObject>().Construct(
                () => InstantiateStringBagAndConstruct(ProjectilePartEnum.Left, position, iceScale, shadowScale, out _).GetComponent<ISliced>()
                ,() => InstantiateStringBagAndConstruct(ProjectilePartEnum.Right, position, iceScale, shadowScale, out _).GetComponent<ISliced>()
                , _destroyTrigger);
            return stringBag;
        }

        public Ice CreateIce(Vector2 position, Vector2 iceScale, Vector2 shadowScale, out Shadow shadow, Transform parent = null)
        {
            Ice ice = InstantiateIceAndConstruct(ProjectilePartEnum.Whole, position, iceScale, shadowScale, out shadow, parent);
            ice.GetComponent<DestroySliceObject>().Construct(_destroyTrigger);
            return ice;
        }

        public Brick CreateBrick(Vector2 position, Vector2 magnetScale, Vector2 shadowScale, out Shadow shadow, Transform parent = null)
        {
            Brick brick = InstantiateBrickAndConstruct(ProjectilePartEnum.Whole, position, magnetScale, shadowScale, out shadow, parent);
            return brick;
        }
        
        public Magnet CreateMagnet(Vector2 position, Vector2 magnetSclae, Vector2 shadowScale, out Shadow shadow, Transform parent = null)
        {
            Magnet magnet = InstantiateMagnetAndConstruct(ProjectilePartEnum.Whole, position, magnetSclae, shadowScale, out shadow, parent);
            magnet.GetComponent<DestroySliceObject>().Construct(_destroyTrigger);
            return magnet;
        }

        public Fruit CreateFruit(FruitType fruitType, Vector2 position, Vector2 fruitScale, Vector2 shadowScale, out Shadow shadow, Transform parent = null)
        {
            Fruit fruit = InstantiateFruitAndConstruct(ProjectilePartEnum.Whole, fruitType, position, fruitScale, shadowScale, out shadow, parent);
            fruit.DestroyNotSliced += _healthSystem.LooseLife;
            
            fruit.GetComponent<TwoPartsSliceObject>().Construct(
                () => InstantiateFruitAndConstruct(ProjectilePartEnum.Left, fruitType, position, fruitScale, shadowScale, out _).GetComponent<ISliced>()
                ,() => InstantiateFruitAndConstruct(ProjectilePartEnum.Right, fruitType, position, fruitScale, shadowScale, out _).GetComponent<ISliced>()
                , _destroyTrigger);
            
            return fruit;
        }
        
        public Bomb CreateBomb(Vector2 position, Vector2 fruitScale, Vector2 shadowScale, out Shadow shadow, Transform parent = null)
        {
            Bomb bomb = InstantiateBombAndConstruct(ProjectilePartEnum.Whole, position, fruitScale, shadowScale, out shadow, parent);

            bomb.GetComponent<TwoPartsSliceObject>().Construct(
                () => InstantiateBombAndConstruct(ProjectilePartEnum.Left, position, fruitScale, shadowScale, out _).GetComponent<ISliced>(), 
                () => InstantiateBombAndConstruct(ProjectilePartEnum.Right, position, fruitScale, shadowScale, out _).GetComponent<ISliced>()
                , _destroyTrigger);
            
            return bomb;
        }
        
        public Heart CreateHeart(Vector2 position, Vector2 fruitScale, Vector2 shadowScale, out Shadow shadow, Transform parent = null)
        {
            Heart heart = InstantiateHeartAndConstruct(ProjectilePartEnum.Whole, position, fruitScale, shadowScale, out shadow, parent);

            heart.GetComponent<TwoPartsSliceObject>().Construct(
                () => InstantiateHeartAndConstruct(ProjectilePartEnum.Left, position, fruitScale, shadowScale, out _).GetComponent<ISliced>(), 
                () => InstantiateHeartAndConstruct(ProjectilePartEnum.Right, position, fruitScale, shadowScale, out _).GetComponent<ISliced>(), _destroyTrigger);
            
            return heart;
        }
        
        public void GetRandomScaleInConfigRange(ProjectileType projectileType, ProjectileConfig projectileConfig, out Vector2 scale)
        {
            float randomScaleValue = projectileConfig.ProjectileScales[projectileType].Scale.GetRandomBound();
            scale = new Vector2(randomScaleValue, randomScaleValue);
        }
        
        public void GetRandomPositionInConfigRange(SpawnAreaData areaData, out Vector2 position)
        {
            position = _screenSettingsProvider
                .ViewportToWorldPosition((areaData.ViewportLeftPosition, areaData.ViewportRightPosition)
                    .GetRandomPointBetween());
        }
        
        private Fruit InstantiateFruitAndConstruct(ProjectilePartEnum projectilePartEnum, FruitType fruitType, Vector2 position,
            Vector2 fruitScale, Vector2 shadowScale, out Shadow createdShadow, Transform parent = null)
        {
            Fruit fruit = CreatePart<Fruit>(ProjectileType.Fruit, projectilePartEnum,
                _projectileConfig.FruitDictionary[fruitType].SpriteData, position, fruitScale, shadowScale,
                out createdShadow, parent);
            
            _projectileContainer.AddToDictionary(ProjectileType.Fruit, fruit.GetComponent<ProjectileObject>());
            fruit.Construct(_projectileConfig.FruitDictionary[fruitType].SliceColor, _particleSystemPlayer);
            return fruit;
        }
        
        private StringBag InstantiateStringBagAndConstruct(ProjectilePartEnum projectilePartEnum, Vector2 position,
            Vector2 fruitScale, Vector2 shadowScale, out Shadow createdShadow, Transform parent = null)
        {
            StringBag stringBag = CreatePart<StringBag>(ProjectileType.StringBag, projectilePartEnum,
                _projectileConfig.BonusesDictionary[BonusesType.StringBag].SpriteData, position, fruitScale, shadowScale,
                out createdShadow, parent);
            
            if(projectilePartEnum == ProjectilePartEnum.Whole)
                stringBag.Construct(_shooter, this, _projectileConfig, _spawnConfig, _bonusesConfig);
            
            return stringBag;
        }
        
        private Ice InstantiateIceAndConstruct(ProjectilePartEnum projectilePartEnum, Vector2 position,
            Vector2 fruitScale, Vector2 shadowScale, out Shadow createdShadow, Transform parent = null)
        {
            Ice ice = CreatePart<Ice>(ProjectileType.Ice, projectilePartEnum,
                _projectileConfig.BonusesDictionary[BonusesType.Ice].SpriteData, position, fruitScale, shadowScale,
                out createdShadow, parent);
            
            ice.Construct(_particleSystemPlayer, _freezer,_bonusesConfig);
            return ice;
        }

        
        private Brick InstantiateBrickAndConstruct(ProjectilePartEnum projectilePartEnum, Vector2 position,
            Vector2 fruitScale, Vector2 shadowScale, out Shadow createdShadow, Transform parent = null)
        {
            Brick brick = CreatePart<Brick>(ProjectileType.Brick, projectilePartEnum,
                _projectileConfig.BonusesDictionary[BonusesType.Brick].SpriteData, position, fruitScale, shadowScale,
                out createdShadow, parent);
            
            brick.Construct(_slicer, _particleSystemPlayer);
            return brick;
        }
        
        private Magnet InstantiateMagnetAndConstruct(ProjectilePartEnum projectilePartEnum, Vector2 position,
            Vector2 fruitScale, Vector2 shadowScale, out Shadow createdShadow, Transform parent = null)
        {
            Magnet magnet = CreatePart<Magnet>(ProjectileType.Magnet, projectilePartEnum,
                _projectileConfig.BonusesDictionary[BonusesType.Magnet].SpriteData, position, fruitScale, shadowScale,
                out createdShadow, parent);
            
            magnet.Construct(_particleSystemPlayer, _bonusesConfig);
            return magnet;
        }
        
        private Bomb InstantiateBombAndConstruct(ProjectilePartEnum projectilePartEnum, Vector2 position,
            Vector2 fruitScale, Vector2 shadowScale, out Shadow createdShadow, Transform parent = null)
        {
            Bomb bomb = CreatePart<Bomb>(ProjectileType.Bomb, projectilePartEnum,
                _projectileConfig.BonusesDictionary[BonusesType.Bomb].SpriteData, position, fruitScale, shadowScale,
                out createdShadow, parent);
            
            bomb.Construct(_particleSystemPlayer, _healthSystem);
            return bomb;
        }


        private Heart InstantiateHeartAndConstruct(ProjectilePartEnum projectilePartEnum, Vector2 position,
            Vector2 fruitScale, Vector2 shadowScale, out Shadow createdShadow, Transform parent = null)
        {
            Heart heart = CreatePart<Heart>(ProjectileType.Heart, projectilePartEnum,
                _projectileConfig.BonusesDictionary[BonusesType.Heart].SpriteData, position, fruitScale, shadowScale,
                out createdShadow, parent);
            
            heart.Construct(_particleSystemPlayer, _healthSystem);
            return heart;
        }
        
        private T CreatePart<T>(ProjectileType projectileType, ProjectilePartEnum projectilePart, SpriteData spriteData, Vector2 position,
            Vector2 fruitScale, Vector2 shadowScale, out Shadow shadow, Transform parent = null)
            where T : MonoBehaviour
        {
            if (parent == null)
                parent = _projectileParenter.transform;
            
            T part = SpawnProjectile<T>(projectileType, projectilePart, position, fruitScale, parent);
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
