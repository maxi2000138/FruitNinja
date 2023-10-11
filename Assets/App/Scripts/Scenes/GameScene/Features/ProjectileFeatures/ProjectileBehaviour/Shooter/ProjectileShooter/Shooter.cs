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

    public Shooter(IProjectileFactory projectileFactory, SpawnAreasContainer spawnAreasContainer, IScreenSettingsProvider screenSettingsProvider
        , ProjectileConfig projectileConfig, ShadowConfig shadowConfig, FruitConfig fruitConfig, GravitationConfig gravitationConfig)
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
        WholeFruit wholeFruit = _projectileFactory.CreateFruitByType(position, type);

        FruitPart[] fruitParts = new FruitPart[2] { wholeFruit.LeftFruitPart, wholeFruit.RightFruitPart };

        Vector2 finalValue = new Vector2(wholeFruit.transform.localScale.x + GetScaleDistance(wholeFruit.LeftFruitPart.SpriteScale.x, _fruitConfig.FruitScaleRange)
        , wholeFruit.transform.localScale.x + GetScaleDistance(wholeFruit.LeftFruitPart.SpriteScale.y, _fruitConfig.FruitScaleRange));
        wholeFruit.ScalerByTime.StartScaling(wholeFruit.transform.localScale,finalValue, GetFlyTimeFromYPosition(position.y));
        SetScalingAndOffseting(GetScaleDistance(wholeFruit.LeftFruitPart.SpriteScale.x, _fruitConfig.FruitScaleRange), GetFlyTimeFromYPosition(position.y), fruitParts);
        SetRotateAndShoot(areaData, angle, wholeFruit.LeftFruitPart, wholeFruit.RightFruitPart);
    }

    private void SetRotateAndShoot(SpawnAreaData areaData, float angle, params FruitPart[] fruitParts)
    {
            Vector2 moveVector = GetMovementVector(areaData, angle);
            moveVector = ConstrainSpeed(FruitSpriteHeight(fruitParts[0]), moveVector);
            
            RotateFruit(fruitParts);
            
            for(int i = 0; i < fruitParts.Length; i++)
                ShootFruit(fruitParts[i].gameObject, moveVector);
    }

    private void SetScalingAndOffseting(float scaleDistance, float flyTime, FruitPart[] fruitPart)
    {
        for (int i = 0; i < fruitPart.Length; i++)
        {
            Vector2 currentScale = fruitPart[i].Shadow.SpriteRenderer.transform.localScale;
            Vector2 deltaScale = new Vector2(currentScale.x + scaleDistance * _shadowConfig.ShadowScaleScaler, currentScale.y + scaleDistance * _shadowConfig.ShadowScaleScaler);
            deltaScale.x = Mathf.Clamp(deltaScale.x, 1f, float.MaxValue);
            deltaScale.y = Mathf.Clamp(deltaScale.y, 1f, float.MaxValue);
            fruitPart[i].StartChangingShadowSpriteScale(currentScale,deltaScale, flyTime);
            Vector2 deltaOffset = new Vector2(_shadowConfig.ShadowDirectionX, _shadowConfig.ShadowDirectionY).normalized * 
                                  Mathf.Clamp(scaleDistance * _shadowConfig.ShadowOffsetScaler, 0, float.MaxValue);
            fruitPart[i].StartChangingShadowOffset(fruitPart[i].transform.localPosition,(Vector2)fruitPart[i].transform.localPosition + deltaOffset , flyTime);
        }
    }

    private float FruitSpriteHeight(FruitPart fruitPart)
    {
        return fruitPart.transform.position.y + fruitPart.SpriteMaxHeight/2;
    }

    public void ShootFruit(GameObject fruit, Vector2 moveVector)
    {
        ForceApplier forceApplier = fruit.GetComponentInChildren<ForceApplier>();
        forceApplier.Shoot(moveVector);
    }

    private void RotateFruit(params FruitPart[] fruitParts)
    {
        float torqueValue = _projectileConfig.TorqueVelocityRange.GetRandomFloatBetween();
        for (int i = 0; i < fruitParts.Length; i++)
        {
            TorqueApplier torqueApplier = fruitParts[i].GetComponentInChildren<TorqueApplier>();
            torqueApplier.AddTorque(torqueValue);
        }
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