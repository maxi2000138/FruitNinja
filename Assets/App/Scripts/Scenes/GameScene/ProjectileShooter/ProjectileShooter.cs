using System.Collections;
using UnityEngine;

public class ProjectileShooter
{
    private float timer = 0;
    private Coroutine _shootCoroutine;
    private readonly SpawnAreasContainer _spawnAreasContainer;
    private readonly CameraFeaturesProvider _cameraFeaturesProvider;
    private readonly ICoroutineRunner _coroutineRunner;
    private readonly GameConfig _gameConfig;
    private readonly ProjectileFactory _projectileFactory;

    public ProjectileShooter(ProjectileFactory projectileFactory, SpawnAreasContainer spawnAreasContainer
        , CameraFeaturesProvider cameraFeaturesProvider, ICoroutineRunner coroutineRunner, GameConfig gameConfig)
    {
        _projectileFactory = projectileFactory;
        _spawnAreasContainer = spawnAreasContainer;
        _cameraFeaturesProvider = cameraFeaturesProvider;
        _coroutineRunner = coroutineRunner;
        _gameConfig = gameConfig;
    }

    public void StartShooting()
    {
        StopShooting();
        _shootCoroutine = _coroutineRunner.StartCoroutine(ShootCoroutine());
    }

    public void StopShooting()
    {
        if(_shootCoroutine != null)
            _coroutineRunner.StopCoroutine(_shootCoroutine);
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

    private IEnumerator ShootCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0.3f, 0.7f));
            SpawnAreaData areaData = _spawnAreasContainer.SpawnAreaHandlers.GetRandomItemByProbability(data => data.Probability);
            float angle = (areaData.ShootMinAngle, areaData.ShootMaxAngle).GetRandomFloatBetween();
            SpawnFruitAndShootByAngle(areaData, angle);
        }
    }
}
