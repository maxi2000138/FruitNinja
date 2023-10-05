using System;
using UnityEngine;

public class ProjectileShooter : IProjectileShooter, IInitializable
{
    private float _highestYValue;
    private readonly IScreenSettingsProvider _screenSettingsProvider;
    private readonly SpawnAreasContainer _spawnAreasContainer;
    private readonly IProjectileFactory _projectileFactory;
    private readonly ProjectileConfig _projectileConfig;
    private readonly IShootPolicy _shootPolicy;
    private readonly GameConfig _gameConfig;

    public ProjectileShooter(IProjectileFactory projectileFactory, SpawnAreasContainer spawnAreasContainer
        , IScreenSettingsProvider screenSettingsProvider, ProjectileConfig projectileConfig, GameConfig gameConfig, IShootPolicy shootPolicy)
    {
        _projectileFactory = projectileFactory;
        _spawnAreasContainer = spawnAreasContainer;
        _screenSettingsProvider = screenSettingsProvider;
        _projectileConfig = projectileConfig;
        _gameConfig = gameConfig;
        _shootPolicy = shootPolicy;
    }

    public void Initialize()
    {
        _highestYValue = _screenSettingsProvider.ViewportToWorldPosition(new Vector2(0, 1)).y;
    }

    public void StartShooting()
    {
        _shootPolicy.NeedShoot += Shoot;
        _shootPolicy.StartWorking();
    }

    public void StopShooting()
    {
        _shootPolicy.NeedShoot -= Shoot;
        _shootPolicy.StopWorking();
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
        fruit.GetComponent<ShootApplier>().Shoot(moveVector);
    }

    private void RotateFruit(Fruit fruit)
    {
        fruit.GetComponent<TorqueApplier>().AddTorque(_projectileConfig.TorqueVelocity.GetRandomFloatBetween());
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
        moveVector *= _projectileConfig.ShootVelocity.GetRandomFloatBetween();
        return moveVector;
    }

    private Vector3 GetRotationVector()
    {
        return new Vector3(0f, 0f, _projectileConfig.TorqueVelocity.GetRandomFloatBetween());
    }

    private Vector2 ConstrainSpeed(float positionY, Vector2 moveVector)
    {
        float maxHeight = _highestYValue - positionY;
        float maxVelocity = Mathf.Sqrt(2 * Mathf.Abs(_gameConfig.UpGravitationalConstant * maxHeight));
        float coef = moveVector.y / maxVelocity;
        if (coef > 1)
        {
            moveVector /= coef;
        }

        return moveVector;
    }
}