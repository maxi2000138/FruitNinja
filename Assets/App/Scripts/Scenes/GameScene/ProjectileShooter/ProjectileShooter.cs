using UnityEngine;

public class ProjectileShooter : IInitializable
{
    private float _highestYValue;
    private readonly SpawnAreasContainer _spawnAreasContainer;
    private readonly CameraFeaturesProvider _cameraFeaturesProvider;
    private readonly ProjectileFactory _projectileFactory;
    private readonly GameConfig _gameConfig;
    private readonly IShootPolicy _shootPolicy;

    public ProjectileShooter(ProjectileFactory projectileFactory, SpawnAreasContainer spawnAreasContainer
        , CameraFeaturesProvider cameraFeaturesProvider, GameConfig gameConfig, IShootPolicy shootPolicy)
    {
        _projectileFactory = projectileFactory;
        _spawnAreasContainer = spawnAreasContainer;
        _cameraFeaturesProvider = cameraFeaturesProvider;
        _gameConfig = gameConfig;
        _shootPolicy = shootPolicy;
    }

    public void Initialize()
    {
        _highestYValue = _cameraFeaturesProvider.ViewportToWorldPosition(new Vector2(0, 1)).y;
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

    private void Shoot()
    {
        SpawnAreaData areaData = _spawnAreasContainer.SpawnAreaHandlers.GetRandomItemByProbability(data => data.Probability);
        float angle = (areaData.ShootMinAngle, areaData.ShootMaxAngle).GetRandomFloatBetween();
        SpawnFruitAndShootByAngle(areaData, angle);
    }

    private void SpawnFruitAndShootByAngle(SpawnAreaData areaData, float angle)
    {
        Vector2 position = _cameraFeaturesProvider
                .ViewportToWorldPosition((areaData.ViewportLeftPosition, areaData.ViewportRightPosition)
                .GetRandomPointBetween());
        GameObject fruit = _projectileFactory.CreateFruit(position);
        Vector2 moveVector = GetMovementVector(areaData, angle);
        moveVector = ConstrainSpeed(fruit.transform.position.y, moveVector);
        fruit.GetComponent<ShootApplier>().Shoot(moveVector);
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
