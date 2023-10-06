using System;
using UnityEngine;

public class Shooter : IShooter, IInitializable
{
    private float _highestYValue;
    private readonly IScreenSettingsProvider _screenSettingsProvider;
    private readonly SpawnAreasContainer _spawnAreasContainer;
    private readonly IProjectileFactory _projectileFactory;
    private readonly ProjectileConfig _projectileConfig;
    private readonly GravitationConfig _gravitationConfig;

    public Shooter(IProjectileFactory projectileFactory, SpawnAreasContainer spawnAreasContainer
        , IScreenSettingsProvider screenSettingsProvider, ProjectileConfig projectileConfig, GravitationConfig gravitationConfig)
    {
        _projectileFactory = projectileFactory;
        _spawnAreasContainer = spawnAreasContainer;
        _screenSettingsProvider = screenSettingsProvider;
        _projectileConfig = projectileConfig;
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
        Vector2 moveVector = GetMovementVector(areaData, angle);
        Vector3 rotationVector = GetRotationVector();
        moveVector = ConstrainSpeed(FruitSpriteHeight(fruit), moveVector);
        RotateFruit(fruit);
        ShootFruit(fruit.gameObject, moveVector);
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

    private Vector3 GetRotationVector()
    {
        return new Vector3(0f, 0f, _projectileConfig.TorqueVelocityRange.GetRandomFloatBetween());
    }

    private Vector2 ConstrainSpeed(float positionY, Vector2 moveVector)
    {
        float maxHeight = _highestYValue - positionY;
        float maxVelocity = Mathf.Sqrt(2 * Mathf.Abs(_gravitationConfig.StartGravityValue * maxHeight));
        float coef = moveVector.y / maxVelocity;
        if (coef > 1)
        {
            moveVector /= coef;
        }

        return moveVector;
    }
}