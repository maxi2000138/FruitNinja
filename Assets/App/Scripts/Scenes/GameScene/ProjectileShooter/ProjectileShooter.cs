using UnityEngine;

public class ProjectileShooter
{
    private float timer = 0;
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
        Vector2 position =
            _cameraFeaturesProvider.ViewportToWorldPosition((areaData.ViewportLeftPosition, areaData.ViewportRightPosition)
                .GetRandomPointBetween());
        GameObject fruit = _projectileFactory.CreateFruit(position);
        Vector2 moveVector = new Vector2(1, 1);
        moveVector.x *= Mathf.Cos(Mathf.Deg2Rad * (areaData.LineAngle + angle));
        moveVector.y *= Mathf.Sin(Mathf.Deg2Rad * (areaData.LineAngle + angle));
        fruit.GetComponent<ShootApplier>().Shoot(moveVector * _gameConfig.ShootForce);
    }
}
