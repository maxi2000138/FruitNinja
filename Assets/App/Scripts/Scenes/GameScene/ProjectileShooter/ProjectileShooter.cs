using System;
using UnityEngine;

public class ProjectileShooter : IProjectileShooter, IInitializable
{
    private float _highestYValue;
    private readonly SpawnAreasContainer _spawnAreasContainer;
    private readonly IScreenSettingsProvider _screenSettingsProvider;
    private readonly IProjectileFactory _projectileFactory;
    private readonly GameConfig _gameConfig;
    private readonly IShootPolicy _shootPolicy;

    public ProjectileShooter(IProjectileFactory projectileFactory, SpawnAreasContainer spawnAreasContainer
        , IScreenSettingsProvider screenSettingsProvider, GameConfig gameConfig, IShootPolicy shootPolicy)
    {
        _projectileFactory = projectileFactory;
        _spawnAreasContainer = spawnAreasContainer;
        _screenSettingsProvider = screenSettingsProvider;
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
        GameObject fruit = _projectileFactory.CreateFruitByType(position, type);
        Vector2 moveVector = GetMovementVector(areaData, angle);
        moveVector = ConstrainSpeed(fruit.transform.position.y, moveVector);
        ShootFruit(fruit, moveVector);
    }

    private void ShootFruit(GameObject fruit, Vector2 moveVector)
    {
        fruit.GetComponent<ShootApplier>().Shoot(moveVector);
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
        moveVector *= _gameConfig.ShootVelocity;
        return moveVector;
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