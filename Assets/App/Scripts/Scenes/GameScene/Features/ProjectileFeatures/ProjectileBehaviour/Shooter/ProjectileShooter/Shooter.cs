using System;
using UnityEngine;

public class Shooter : IShooter, IInitializable
{
    private float _highestYValue;
    private readonly IScreenSettingsProvider _screenSettingsProvider;
    private readonly SpawnAreasContainer _spawnAreasContainer;
    private readonly IProjectileFactory _projectileFactory;
    private readonly GravitationConfig _gravitationConfig;
    private readonly SpawnConfig _spawnConfig;
    private readonly ProjectileConfig _projectileConfig;
    private readonly ShadowConfig _shadowConfig;
    private readonly FruitConfig _fruitConfig;

    public Shooter(IProjectileFactory projectileFactory, SpawnAreasContainer spawnAreasContainer, IScreenSettingsProvider screenSettingsProvider
        , ProjectileConfig projectileConfig, ShadowConfig shadowConfig, FruitConfig fruitConfig, GravitationConfig gravitationConfig, SpawnConfig spawnConfig)
    {
        _projectileFactory = projectileFactory;
        _spawnAreasContainer = spawnAreasContainer;
        _screenSettingsProvider = screenSettingsProvider;
        _projectileConfig = projectileConfig;
        _shadowConfig = shadowConfig;
        _fruitConfig = fruitConfig;
        _gravitationConfig = gravitationConfig;
        _spawnConfig = spawnConfig;
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
        GetRandomTypeAndPosition(areaData, _spawnConfig, out var position, out var type);
        WholeFruit wholeFruit = _projectileFactory.CreateFruitByType(position, type);

        Vector2 finalValue = new Vector2(wholeFruit.transform.localScale.x + GetScaleDistance(wholeFruit.LeftFruitPart.SpriteScale.x, _fruitConfig.FruitScaleRange)
        , wholeFruit.transform.localScale.x + GetScaleDistance(wholeFruit.LeftFruitPart.SpriteScale.y, _fruitConfig.FruitScaleRange));
        wholeFruit.ScalerByTime.StartScaling(wholeFruit.transform.localScale,finalValue, GetFlyTimeFromYPosition(position.y));
        SetScalingAndOffseting(GetScaleDistance(wholeFruit.LeftFruitPart.SpriteScale.x, _fruitConfig.FruitScaleRange), GetFlyTimeFromYPosition(position.y), wholeFruit.LeftFruitPart);
        SetScalingAndOffseting(GetScaleDistance(wholeFruit.LeftFruitPart.SpriteScale.x, _fruitConfig.FruitScaleRange), GetFlyTimeFromYPosition(position.y), wholeFruit.RightFruitPart);

        RotateFruit(wholeFruit);
        ShootFruit(areaData, angle, wholeFruit);
    }

    private void ShootFruit(SpawnAreaData areaData, float angle, WholeFruit wholeFruit)
    {
        Vector2 moveVector = GetRandomMovementVector(areaData, angle);
        moveVector = ConstrainSpeed(FruitSpriteHeight(wholeFruit.LeftFruitPart), moveVector);
        ShootFruit(wholeFruit.PhysicsOperationOrder.gameObject, moveVector);
    }

    private void RotateFruit(WholeFruit wholeFruit)
    {
        float torqueValue = _projectileConfig.TorqueVelocityRange.GetRandomFloatBetween();
        TorqueApplier torqueApplier = wholeFruit.TorqueApplier;
        torqueApplier.AddTorque(torqueValue);
    }

    private void SetScalingAndOffseting(float scaleDistance, float flyTime, FruitPart fruitPart)
    {
        Vector2 currentScale = fruitPart.Shadow.SpriteRenderer.transform.localScale;
        Vector2 finalScale = new Vector2(currentScale.x + scaleDistance * _shadowConfig.ShadowScaleScaler, currentScale.y + scaleDistance * _shadowConfig.ShadowScaleScaler);
        finalScale.x = Mathf.Clamp(finalScale.x, 1f, float.MaxValue);
        finalScale.y = Mathf.Clamp(finalScale.y, 1f, float.MaxValue);
        fruitPart.StartChangingShadowSpriteScale(currentScale,finalScale, flyTime);
        
        Vector2 deltaOffset = new Vector2(_shadowConfig.ShadowDirectionX, _shadowConfig.ShadowDirectionY).normalized * Mathf.Clamp(scaleDistance * _shadowConfig.ShadowOffsetScaler, 0, float.MaxValue);
        fruitPart.StartChangingShadowOffset(fruitPart.transform.localPosition,(Vector2)fruitPart.transform.localPosition + deltaOffset , flyTime);
    }

    private float FruitSpriteHeight(FruitPart fruitPart)
    {
        return fruitPart.transform.position.y + fruitPart.SpriteMaxHeight/2;
    }

    public void ShootFruit(GameObject fruit, Vector2 moveVector)
    {
        ForceApplier forceApplier = fruit.GetComponentInChildren<ForceApplier>();
        forceApplier.AddForce(moveVector);
    }

    private void GetRandomTypeAndPosition(SpawnAreaData areaData, SpawnConfig spawnConfig, out Vector2 position, out FruitType type)
    {
        position = _screenSettingsProvider
            .ViewportToWorldPosition((areaData.ViewportLeftPosition, areaData.ViewportRightPosition)
                .GetRandomPointBetween());
        type = spawnConfig.FruitTypes.GetRandomItem();
    }

    private Vector2 GetRandomMovementVector(SpawnAreaData areaData, float angle)
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