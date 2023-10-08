using System;
using UnityEngine;

public class Shooter : IShooter, IInitializable
{
    private float _highestYValue;
    private readonly IScreenSettingsProvider _screenSettingsProvider;
    private readonly SpawnAreasContainer _spawnAreasContainer;
    private readonly IProjectileFactory _projectileFactory;
    private readonly GravitationConfig _gravitationConfig;
    private readonly ProjectileConfig _projectileConfig;
    private readonly ShadowConfig _shadowConfig;
    private readonly FruitConfig _fruitConfig;

    public Shooter(IProjectileFactory projectileFactory, SpawnAreasContainer spawnAreasContainer
        , IScreenSettingsProvider screenSettingsProvider, ProjectileConfig projectileConfig, ShadowConfig shadowConfig, FruitConfig fruitConfig, GravitationConfig gravitationConfig)
    {
        _projectileFactory = projectileFactory;
        _spawnAreasContainer = spawnAreasContainer;
        _screenSettingsProvider = screenSettingsProvider;
        _projectileConfig = projectileConfig;
        _shadowConfig = shadowConfig;
        _fruitConfig = fruitConfig;
        _gravitationConfig = gravitationConfig;
    }

    public void Initialize()
    {
        _highestYValue = _screenSettingsProvider.ViewportToWorldPosition(new Vector2(0, 1)).y;
    }

    public void Shoot()
    {
        SpawnAreaData areaData = _spawnAreasContainer.SpawnAreaHandlers.GetRandomItemByProbability(data => data.Probability);
        float angle = (areaData.ShootMinAngle, areaData.ShootMaxAngle).GetRandomFloatBetween();
        SpawnFruitAndShootByAngle(areaData, angle);
    }

    private void SpawnFruitAndShootByAngle(SpawnAreaData areaData, float angle)
    {
        GetRandomTypeAndPosition(areaData, out var position, out var type);
        Fruit fruit = _projectileFactory.CreateFruitByType(position, type);
        SetScalingAndOffseting(fruit, GetScaleDistance(fruit.SpriteScale, _fruitConfig.FruitScaleRange), GetFlyTimeFromYPosition(position.y));
        SetRotateAndShoot(areaData, angle, fruit);
    }

    private void SetRotateAndShoot(SpawnAreaData areaData, float angle, Fruit fruit)
    {
        Vector2 moveVector = GetMovementVector(areaData, angle);
        moveVector = ConstrainSpeed(FruitSpriteHeight(fruit), moveVector);
        RotateFruit(fruit);
        ShootFruit(fruit.gameObject, moveVector);
    }

    private void SetScalingAndOffseting(Fruit fruit, float scaleDistance, float flyTime)
    {
        fruit.StartChangingFruitSpriteScale(fruit.SpriteScale + scaleDistance, flyTime);
        float deltaScale = fruit.Shadow.SpriteRenderer.transform.localScale.x + scaleDistance * _shadowConfig.ShadowScaleScaler;
        deltaScale = Mathf.Clamp(deltaScale, 1f, float.MaxValue);
        fruit.StartChangingShadowSpriteScale(deltaScale, flyTime);
        fruit.StartChangingShadowOffset(
            new Vector2(_shadowConfig.ShadowDirectionX, _shadowConfig.ShadowDirectionY).normalized
            ,Mathf.Clamp(scaleDistance * _shadowConfig.ShadowOffsetScaler, 0, float.MaxValue), flyTime);
    }

    private float FruitSpriteHeight(Fruit fruit)
    {
        return fruit.transform.position.y + fruit.SpriteMaxHeight/2;
    }

    private void ShootFruit(GameObject fruit, Vector2 moveVector)
    {
        ShootApplier shootApplier = fruit.GetComponentInChildren<ShootApplier>();
        shootApplier.Shoot(moveVector);
    }

    private void RotateFruit(Fruit fruit)
    {
        TorqueApplier torqueApplier = fruit.GetComponentInChildren<TorqueApplier>();
        torqueApplier.AddTorque(_projectileConfig.TorqueVelocityRange.GetRandomFloatBetween());
    }

    private void GetRandomTypeAndPosition(SpawnAreaData areaData, out Vector2 position, out FruitType type)
    {
        position = _screenSettingsProvider
            .ViewportToWorldPosition((areaData.ViewportLeftPosition, areaData.ViewportRightPosition)
                .GetRandomPointBetween());
        type = (FruitType)((0, Enum.GetNames(typeof(FruitType)).Length).GetRandomIntBetween());
    }

    private Vector2 GetMovementVector(SpawnAreaData areaData, float angle)
    {
        Vector2 moveVector = new Vector2(1, 1);
        moveVector.x *= Mathf.Cos(Mathf.Deg2Rad * (areaData.LineAngle + angle));
        moveVector.y *= Mathf.Sin(Mathf.Deg2Rad * (areaData.LineAngle + angle));
        moveVector *= _projectileConfig.ShootVelocityRange.GetRandomFloatBetween();
        return moveVector;
    }
    
    private Vector2 ConstrainSpeed(float positionY, Vector2 moveVector)
    {
        float maxHeight = GetPathHeight(positionY);
        float neededYVelocity = GetNeededYVelocityForHeight(maxHeight);
        float coef = moveVector.y / neededYVelocity;
        if (coef > 1)
        {
            moveVector /= coef;
        }

        return moveVector;
    }

    private float GetScaleDistance(float currentScale, Vector2 scaleRange)
    {
        float downScaleDistance = Mathf.Abs(scaleRange.x - currentScale);
        float upScaleDistance = Mathf.Abs(scaleRange.y - currentScale);
        return downScaleDistance > upScaleDistance ? scaleRange.x - currentScale : scaleRange.y - currentScale;
    }
    
    private float GetFlyTimeFromYPosition(float yPosition) =>
        GetFlyTimeFromVelocity(GetNeededYVelocityForHeight(GetPathHeight(yPosition)));

    private float GetFlyTimeFromVelocity(float yVelocity) => 
        yVelocity * 2 / Mathf.Abs(_gravitationConfig.StartGravityValue);

    private float GetPathHeight(float positionY) => 
        _highestYValue - positionY;

    private float GetNeededYVelocityForHeight(float maxHeight) => 
        Mathf.Sqrt(2 * Mathf.Abs(_gravitationConfig.StartGravityValue * maxHeight));

}